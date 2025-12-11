using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text.Json.Serialization;
using GoodFish.DAL.Models.Interfaces;

namespace GoodFish.DAL.Models
{
    [Table("payment_methods")]
    [DisplayName("Способы оплаты")]
    public class PaymentMethod : IEntity
    {
        [Key]
        [Column("method_id")]
        public int ID { get; set; }
        [MaxLength(50)]
        [Column("method_name")]

        public required string MethodName { get; set; }
        [JsonIgnore]
        public ICollection<Order> Orders { get; set; } = [];


        private static string? _displayNameCache;
        public static string GetDisplayName()
        {
            if (_displayNameCache is not null)
                return _displayNameCache;
            Type entityType = typeof(PaymentMethod);
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
