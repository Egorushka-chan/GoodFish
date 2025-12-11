using GoodFish.API.Controllers.Default;
using GoodFish.BLL.Services;
using GoodFish.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace GoodFish.API.Controllers.Entity
{
    [Route("api/[controller]")]
    public class OrderItemController(IBaseCRUDEntityService<OrderItem> service) : CRUDGenericController<OrderItem>(service)
    {
    }
}
