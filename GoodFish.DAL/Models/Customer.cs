using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text.Json.Serialization;
using GoodFish.DAL.Models.Interfaces;

namespace GoodFish.DAL.Models
{
    [Table("customers")]
    [DisplayName("Заказчики")]
    public class Customer : IEntity
    {
        [Key]
        [Column("customer_id")]
        public int ID { get; set; }
        [Column("full_name")]
        [MaxLength(150)]
        public required string FullName { get; set; }
        [Column("phone")]
        [MaxLength(20)]
        public required string Phone { get; set; }
        [MaxLength(150)]
        public string? Email { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonIgnore]
        public ICollection<CustomerAddress> Addresses { get; set; } = [];
        [JsonIgnore]
        public ICollection<CartItem> CartItems { get; set; } = [];
        [JsonIgnore]
        public ICollection<Order> Orders { get; set; } = [];


        private static string? _displayNameCache;
        public static string GetDisplayName()
        {
            if (_displayNameCache is not null)
                return _displayNameCache;
            Type entityType = typeof(Customer);
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
