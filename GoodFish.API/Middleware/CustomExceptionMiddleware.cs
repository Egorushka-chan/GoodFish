using System.Net;
using System.Text.Json;
using GoodFish.BLL.Models;

namespace GoodFish.API.Middleware
{
    /// <summary>
    /// Middleware, который обрабатывает ошибки в HTTP ответы 
    /// </summary>
    public class CustomExceptionMiddleware
    {
        private const string UNEXPECTED_ERROR_TEXT = "Непредвиденная ошибка";

        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<CustomExceptionMiddleware> _logger;

        public CustomExceptionMiddleware(RequestDelegate next, IWebHostEnvironment environment, ILogger<CustomExceptionMiddleware> logger)
        {
            _next = next;
            _environment = environment;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error {exceptionName}:", exception.GetType().Name);
                DefaultErrorMessage response = exception switch
                {
                    EntityNotFoundException ex => DefaultErrorMessage.FromEntityNotFound(ex),
                    BadRequestException ex => DefaultErrorMessage.FromBadRequest(ex),
                    _ => new DefaultErrorMessage()
                    {
                        Title = UNEXPECTED_ERROR_TEXT,
                        Status = (int)HttpStatusCode.InternalServerError,
                        TimeStamp = DateTime.Now,
                        Details = exception.Message
                    }
                };

                context.Response.StatusCode = response.Status;
                context.Response.ContentType = "application/problem+json";

                if (response.Status == (int)HttpStatusCode.InternalServerError && _environment.IsDevelopment())
                {
                    // Если требуется выводить ошибку в ответе
                    await context.Response.WriteAsync(JsonSerializer.Serialize(exception));
                    return;
                }

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}
