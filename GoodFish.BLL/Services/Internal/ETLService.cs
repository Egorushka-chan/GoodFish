using System.Text;
using System.Text.Json;
using CsvHelper;
using GoodFish.BLL.Models;
using GoodFish.BLL.Models.ETL;
using GoodFish.DAL.Models;
using GoodFish.DAL.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GoodFish.BLL.Services.Internal
{
    internal class ETLService(ILogger<ETLService> logger, IBaseRepository<Customer> customerRepository) : IETLService
    {
        public async Task ProcessCustomersAsync(IFormFile file)
        {
            // Извлекаем сырые данные из файла
            List<ETLCustomerDTO> customersRawData = await ExtractCustomersAsync(file);

            // Начинаем валидацию данных
            int totalRecords = customersRawData.Count;
            int totalPassed = 0;
            List<string> errors = [];
            List<ETLCustomerDTO> validCustomersRaw = [];
            foreach (var customerRaw in customersRawData)
            {
                // Пропускаем некорректные записи
                if (string.IsNullOrWhiteSpace(customerRaw.ID))
                {
                    logger.LogInformation("Пропуск некорректной записи заказчика с пустым ID");
                    errors.Add($"Пропуск некорректной записи заказчика с пустым ID");
                    continue;
                }

                if(long.TryParse(customerRaw.ID, out _) is false)
                {
                    logger.LogInformation("Пропуск записи заказчика с некорректным ID: {CustomerID}", customerRaw.ID);
                    errors.Add($"Пропуск записи заказчика с некорректным ID: {customerRaw.ID}");
                    continue;
                }

                // Проверяем, есть ли уже такой заказчик в БД
                var existingCustomer = await customerRepository.GetByIdAsync(long.Parse(customerRaw.ID), CancellationToken.None);
                if (existingCustomer != null)
                {
                    logger.LogInformation("Заказчик с ID: {CustomerID} уже существует. Пропуск.", customerRaw.ID);
                    errors.Add($"Заказчик с ID: {customerRaw.ID} уже существует. Пропуск.");
                    continue;
                }

                if (string.IsNullOrEmpty(customerRaw.FullName) || string.IsNullOrEmpty(customerRaw.Email) || string.IsNullOrEmpty(customerRaw.Gender))
                {
                    logger.LogInformation("Пропуск записи заказчика с ID: {CustomerID} из-за отсутствия обязательных полей.", customerRaw.ID);
                    errors.Add($"Пропуск записи заказчика с ID: {customerRaw.ID} из-за отсутствия обязательных полей.");
                    continue;
                }

                if (customerRaw.Email == null || !customerRaw.Email.Contains("@"))
                {
                    logger.LogInformation("Пропуск записи заказчика с ID: {CustomerID} из-за некорректного email.", customerRaw.ID);
                    errors.Add($"Пропуск записи заказчика с ID: {customerRaw.ID} из-за некорректного email.");
                    continue;
                }

                if (DateTime.TryParse(customerRaw.CreatedAt, out _) is false)
                {
                    logger.LogInformation("Пропуск записи заказчика с ID: {CustomerID} из-за некорректной даты создания.", customerRaw.ID);
                    errors.Add($"Пропуск записи заказчика с ID: {customerRaw.ID} из-за некорректной даты создания.");
                    continue;
                }

                if (customerRaw?.Gender != "муж" && customerRaw?.Gender != "жен")
                {
                    logger.LogInformation("Пропуск записи заказчика с ID: {CustomerID} из-за некорректного значения пола.", customerRaw!.ID);
                    errors.Add($"Пропуск записи заказчика с ID: {customerRaw.ID} из-за некорректного значения пола.");
                    continue;
                }

                totalPassed++;
                validCustomersRaw.Add(customerRaw);
            }

            // Начинаем преобразование данных
            List<Customer> customers = [];
            foreach (var validCustomerRaw in validCustomersRaw)
            {
                int id = int.Parse(validCustomerRaw.ID!);
                string fullName = validCustomerRaw.FullName!;
                string phone = validCustomerRaw.Phone!;
                string email = validCustomerRaw.Email!;
                DateTime createdAt = DateTime.Parse(validCustomerRaw.CreatedAt!);
                createdAt = DateTime.SpecifyKind(createdAt, DateTimeKind.Utc);
                bool gender = validCustomerRaw.Gender == "муж";
                customers.Add(new Customer
                {
                    ID = id,
                    FullName = fullName,
                    Phone = phone,
                    Email = email,
                    CreatedAt = createdAt,
                    Gender = gender
                });
            }

            // Начинаем обработку данных, сохраняем в БД и сохраняем логи
            int totalInserted = 0;
            foreach (var customer in customers)
            {
                try
                {
                    await customerRepository.CreateAsync(customer, CancellationToken.None);
                    totalInserted++;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Ошибка при сохранении заказчика с ID: {CustomerID} в БД.", customer.ID);
                    errors.Add($"Ошибка при сохранении заказчика с ID: {customer.ID} в БД. {ex.Message}");
                }
            }
            ETLProcessingLog processingLog = new ETLProcessingLog
            {
                TimeStamp = DateTime.Now,
                Statistics = [new ETLProcessingLog.ETLProcessingStatistics
                {
                    EntityName = "customers",
                    TotalRecords = totalRecords,
                    ValidRecords = totalPassed,
                    ErrorRecords = totalRecords - totalPassed,
                    CreatedRecords = totalInserted,
                    Errors = [.. errors]
                }],
                Summary = new ETLProcessingLog.ETLProcessingSummary
                {
                    TotalRecords = totalRecords,
                    TotalValid = totalPassed,
                    TotalCreated = totalInserted,
                    TotalError = totalRecords - totalPassed
                }
            };

            // Сохраняем лог обработки в JSON файл
            string logFileName = $"ETL_Log_{DateTime.Now:yyyyMMdd_HHmmss}.json";
            string logDir = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
            Directory.CreateDirectory(logDir); // создаём папку если нет
            string logPath = Path.Combine(logDir, logFileName);
            string json = JsonSerializer.Serialize(processingLog);
            await File.WriteAllTextAsync(logPath, json, encoding: Encoding.UTF8);
        }

        private async Task<List<ETLCustomerDTO>> ExtractCustomersAsync(IFormFile file)
        {
            logger.LogInformation("Начало обработки файла с заказчиками: {FileName}", file.FileName);
            if (GetFileFormat(file.FileName) != "csv")
            {
                logger.LogInformation("Неподдерживаемый формат файла: {FileName}", file.FileName);
                throw new BadRequestException("Неподдерживаемый формат файла. Только .csv файлы.");
            }

            // Скачиваем файл в память
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            // Обрабатываем CSV
            using var reader = new StreamReader(memoryStream, Encoding.UTF8);
            using var csvReader = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture);
            List<ETLCustomerDTO> customersRawData = [];
            csvReader.Read();
            while (csvReader.Read())
            {
                string? id = csvReader.GetField(0);
                string? fullName = csvReader.GetField(1);
                string? phone = csvReader.GetField(2);
                string? email = csvReader.GetField(3);
                string? createdAtStr = csvReader.GetField(4);
                string? genderStr = csvReader.GetField(5);
                customersRawData.Add(new ETLCustomerDTO
                {
                    ID = id,
                    FullName = fullName,
                    Phone = phone,
                    Email = email,
                    CreatedAt = createdAtStr,
                    Gender = genderStr
                });
            }
            logger.LogInformation("Завершение обработки файла с заказчиками: {FileName}. Извлечено записей: {RecordCount}", file.FileName, customersRawData.Count);
            return customersRawData;
        }

        /// <summary>
        /// Возвращает расширение файла (при наличии)
        /// </summary>
        /// <param name="fullFileName">Полное имя файла (вместе с возможным расширением)</param>
        public static string? GetFileFormat(string fullFileName)
        {
            var ind = fullFileName.LastIndexOf('.');

            if (ind != -1 && fullFileName.Length > ind + 1)
            {
                return fullFileName.Substring(ind + 1).ToLower();
            }

            return null;
        }
    }
}
