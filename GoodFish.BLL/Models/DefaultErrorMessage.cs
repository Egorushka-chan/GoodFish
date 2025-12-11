namespace GoodFish.BLL.Models
{
    /// <summary>
    /// Базовое сообщение об ошибке
    /// </summary>
    /// <remarks>
    /// Структура похожа на <see cref="ProblemDetails"/>
    /// </remarks>
    public class DefaultErrorMessage
    {
        /// <summary>
        /// Общее описание ошибки
        /// </summary>
        public required string Title { get; set; }
        /// <summary>
        /// HTTP статус-код
        /// </summary>
        public required int Status { get; set; }
        /// <summary>
        /// Время генерации ошибки
        /// </summary>
        public DateTime TimeStamp { get; set; }
        /// <summary>
        /// Конкретное описание ошибки
        /// </summary>
        public string Details { get; set; }
        /// <summary>
        /// Поле с ошибками валидации 
        /// </summary>
        public Dictionary<string, string[]> Errors { get; set; }

        public static DefaultErrorMessage FromBadRequest(BadRequestException ex)
        {
            DefaultErrorMessage message = new()
            {
                Title = ex.Title,
                Status = ex.StatusCode,
                Errors = ex.Errors?.Select(x => new KeyValuePair<string, string[]>(x.Key, [.. x.Value]))?.ToDictionary(),
                Details = ex.Message,
                TimeStamp = DateTime.Now
            };
            return message;
        }

        public static DefaultErrorMessage FromEntityNotFound(EntityNotFoundException ex)
        {
            return new()
            {
                Title = ex.Title,
                Status = ex.StatusCode,
                Details = ex.Message,
                TimeStamp = DateTime.Now
            };
        }
    }
}
