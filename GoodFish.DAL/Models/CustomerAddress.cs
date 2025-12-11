using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text.Json.Serialization;
using GoodFish.DAL.Models.Interfaces;

namespace GoodFish.DAL.Models
{
    [Table("customer_addresses")]
    [DisplayName("Адрес заказчиков")]
    public class CustomerAddress : IEntity
    {
        [Key]
        [Column("address_id")]
        public int ID { get; set; }
        [Column("customer_id")]
        public int CustomerId { get; set; }
        [Column("city")]
        [MaxLength(100)]
        public required string City { get; set; }
        [Column("street")]
        [MaxLength(200)]
        public required string Street { get; set; }
        [Column("house")]
        [MaxLength(20)]
        public required string House { get; set; }
        [Column("apartment")]
        [MaxLength(20)]
        public string? Apartment { get; set; }
        [Column("is_default")]
        public bool IsDefault { get; set; } = false;

        [JsonIgnore]
        public Customer? Customer { get; set; }
        [JsonIgnore]
        public ICollection<Order> Orders { get; set; } = [];


        private static string? _displayNameCache;
        public static string GetDisplayName()
        {
            if (_displayNameCache is not null)
                return _displayNameCache;
            Type entityType = typeof(CustomerAddress);
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
