using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text.Json.Serialization;
using GoodFish.DAL.Models.Interfaces;

namespace GoodFish.DAL.Models
{
    [Table("dish_categories")]
    [DisplayName("Категории блюд")]
    public class DishCategory : IEntity
    {
        [Key]
        [Column("category_id")]
        public int ID { get; set; }
        [Column("category_name")]
        [MaxLength(50)]
        public required string CategoryName { get; set; }

        [JsonIgnore]
        public ICollection<Dish> Dishes { get; set; } = [];


        private static string? _displayNameCache;
        public static string GetDisplayName()
        {
            if (_displayNameCache is not null)
                return _displayNameCache;
            Type entityType = typeof(DishCategory);
            DisplayNameAttribute? attr = entityType.GetCustomAttribute<DisplayNameAttribute>();
            if (attr != null)
            {
                _displayNameCache = attr.DisplayName;
                return attr.DisplayName;
            }
            else
            {
                _displayNameCache = entityType.Name;
                return entityType.Name;
            }
        }
    }
}