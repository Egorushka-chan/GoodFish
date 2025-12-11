using GoodFish.DAL.Models;
using GoodFish.DAL.Repositories;
using GoodFish.DAL.Repositories.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace GoodFish.DAL.DI
{
    public static class InjectorDAL
    {
        public static IServiceCollection InjectDataAccessLayer(this IServiceCollection services, string connectionString)
        {
            services.AddNpgsql<MainDbContext>(connectionString);
            services.RegisterRepositories();
            return services;
        }

        private static void RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<IBaseRepository<CartItem>, BaseRepository<CartItem>>();
            services.AddScoped<IBaseRepository<Customer>, BaseRepository<Customer>>();
            services.AddScoped<IBaseRepository<CustomerAddress>, BaseRepository<CustomerAddress>>();
            services.AddScoped<IBaseRepository<Dish>, BaseRepository<Dish>>();
            services.AddScoped<IBaseRepository<DishCategory>, BaseRepository<DishCategory>>();
            services.AddScoped<IBaseRepository<Order>, BaseRepository<Order>>();
            services.AddScoped<IBaseRepository<OrderItem>, BaseRepository<OrderItem>>();
            services.AddScoped<IBaseRepository<PaymentMethod>, BaseRepository<PaymentMethod>>();
        }
    }
}
