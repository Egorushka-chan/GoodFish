namespace GoodFish.DAL.Models.Interfaces
{
    public interface IEntity
    {
        public int ID { get; set; }

        abstract static string GetDisplayName();
    }
}
