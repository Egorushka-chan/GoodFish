using GoodFish.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace GoodFish.DAL
{
    internal class MainDbContext(DbContextOptions<MainDbContext> options) : DbContext(options)
    {
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerAddress> CustomerAddresses { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<DishCategory> DishCategories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
    }
}
