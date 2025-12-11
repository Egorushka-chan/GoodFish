using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text.Json.Serialization;
using GoodFish.DAL.Models.Interfaces;

namespace GoodFish.DAL.Models
{
    [Table("orders")]
    [DisplayName("Заказы")]
    public class Order : IEntity
    {
        [Key]
        [Column("order_id")]
        public int ID { get; set; }
        [Column("customer_id")]
        public int CustomerId { get; set; }
        [Column("address_id")]
        public int AddressId { get; set; }
        [Column("payment_method_id")]
        public int? PaymentMethodId { get; set; }

        [Column("order_status")]
        [MaxLength(50)]
        public string? OrderStatus { get; set; }
        [Column("total_price")]
        public decimal TotalPrice { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonIgnore]
        public Customer? Customer { get; set; }
        [JsonIgnore]
        public CustomerAddress? Address { get; set; }
        [JsonIgnore]
        public PaymentMethod? PaymentMethod { get; set; }
        [JsonIgnore]
        public ICollection<OrderItem> Items { get; set; } = [];


        private static string? _displayNameCache;
        public static string GetDisplayName()
        {
            if (_displayNameCache is not null)
                return _displayNameCache;
            Type entityType = typeof(Order);
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
