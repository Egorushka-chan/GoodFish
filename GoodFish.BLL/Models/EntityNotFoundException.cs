namespace GoodFish.BLL.Models
{
    /// <summary>
    /// Ошибка, выбрасывается в случае, если данные по переданному ключу не найдены
    /// </summary>
    public class EntityNotFoundException : Exception
    {
        public string Title { get; set; } = "По переданному ключу ничего не найдено";
        public int StatusCode { get; set; } = 404;

        public EntityNotFoundException()
        : base("Entity not found.")
        { }

        public EntityNotFoundException(string message)
            : base(message)
        { }

        public EntityNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
