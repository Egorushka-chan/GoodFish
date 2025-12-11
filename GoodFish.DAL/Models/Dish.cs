using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text.Json.Serialization;
using GoodFish.DAL.Models.Interfaces;

namespace GoodFish.DAL.Models
{
    [Table("dishes")]
    [DisplayName("Блюда")]
    public class Dish : IEntity
    {
        [Key]
        [Column("dish_id")]
        public int ID { get; set; }
        [Column("name")]
        [MaxLength(150)]
        public required string Name { get; set; }
        [Column("description")]
        public string? Description { get; set; }
        [Column("price")]
        public decimal Price { get; set; }
        [Column("category_id")]
        public int? CategoryId { get; set; }
        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [JsonIgnore]
        public DishCategory? Category { get; set; }
        [JsonIgnore]
        public ICollection<CartItem> CartItems { get; set; } = [];
        [JsonIgnore]
        public ICollection<OrderItem> OrderItems { get; set; } = [];


        private static string? _displayNameCache;
        public static string GetDisplayName()
        {
            if (_displayNameCache is not null)
                return _displayNameCache;
            Type entityType = typeof(Dish);
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
