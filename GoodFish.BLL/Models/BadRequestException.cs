namespace GoodFish.BLL.Models
{
    /// <summary>
    /// Ошибка, выбрасывается в случаях, когда получены некорректные данные
    /// </summary>
    public class BadRequestException : Exception
    {
        public string Title { get; set; } = "Произошла одна или более ошибок валидации.";
        public int StatusCode { get; set; } = 400;
        public Dictionary<string, List<string>> Errors { get; set; } = [];

        public BadRequestException()
        {

        }

        public BadRequestException(Dictionary<string, List<string>> errors) : base()
        {
            Errors = errors;
        }

        public BadRequestException(string message) : base(message)
        {

        }

        public BadRequestException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
