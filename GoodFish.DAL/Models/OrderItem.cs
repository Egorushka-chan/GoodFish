using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text.Json.Serialization;
using GoodFish.DAL.Models.Interfaces;

namespace GoodFish.DAL.Models
{
    [Table("order_items")]
    [DisplayName("Позиции заказов")]
    public class OrderItem : IEntity
    {
        [Key]
        [Column("order_item_id")]
        public int ID { get; set; }
        [Column("order_id")]
        public int OrderId { get; set; }
        [Column("dish_id")]
        public int DishId { get; set; }
        [Column("quantity")]
        public int Quantity { get; set; }
        [Column("price")]
        public decimal Price { get; set; }

        [JsonIgnore]
        public Order? Order { get; set; }
        [JsonIgnore]
        public Dish? Dish { get; set; }


        private static string? _displayNameCache;
        public static string GetDisplayName()
        {
            if (_displayNameCache is not null)
                return _displayNameCache;
            Type entityType = typeof(OrderItem);
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
