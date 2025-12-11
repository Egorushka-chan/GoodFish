using GoodFish.BLL.Services;
using GoodFish.BLL.Services.Internal;
using GoodFish.DAL.Models;
using Microsoft.Extensions.DependencyInjection;

namespace GoodFish.BLL.DI
{
    public static class InjectorBLL
    {
        public static IServiceCollection InjectBusinessLogicLayer(this IServiceCollection services)
        {
            services.AddScoped<IBaseCRUDEntityService<CartItem>, BaseCRUDEntityService<CartItem>>();
            services.AddScoped<IBaseCRUDEntityService<Customer>, BaseCRUDEntityService<Customer>>();
            services.AddScoped<IBaseCRUDEntityService<CustomerAddress>, BaseCRUDEntityService<CustomerAddress>>();
            services.AddScoped<IBaseCRUDEntityService<Dish>, BaseCRUDEntityService<Dish>>();
            services.AddScoped<IBaseCRUDEntityService<DishCategory>, BaseCRUDEntityService<DishCategory>>();
            services.AddScoped<IBaseCRUDEntityService<Order>, BaseCRUDEntityService<Order>>();
            services.AddScoped<IBaseCRUDEntityService<OrderItem>, BaseCRUDEntityService<OrderItem>>();
            services.AddScoped<IBaseCRUDEntityService<PaymentMethod>, BaseCRUDEntityService<PaymentMethod>>();
            return services;
        }
    }
}
