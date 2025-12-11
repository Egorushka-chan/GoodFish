using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text.Json.Serialization;
using GoodFish.DAL.Models.Interfaces;

namespace GoodFish.DAL.Models
{
    [Table("cart_items")]
    [DisplayName("Корзина")]
    public class CartItem : IEntity
    {
        [Key]
        [Column("cart_item_id")]
        public int ID { get; set; }
        [Column("customer_id")]
        public int CustomerId { get; set; }
        [Column("dish_id")]
        public int DishId { get; set; }
        [Column("quantity")]
        public int Quantity { get; set; }
        [Column("added_at")]
        public DateTime? AddedAt { get; set; }

        [JsonIgnore]
        public Customer? Customer { get; set; }
        [JsonIgnore]
        public Dish? Dish { get; set; }


        private static string? _displayNameCache;
        public static string GetDisplayName()
        {
            if (_displayNameCache is not null)
                return _displayNameCache;
            Type entityType = typeof(CartItem);
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
