using GoodFish.API.Controllers.Default;
using GoodFish.BLL.Services;
using GoodFish.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace GoodFish.API.Controllers
{
    [Route("api/[controller]")]
    public class CustomerAddressController(IBaseCRUDEntityService<CustomerAddress> service) : CRUDGenericController<CustomerAddress>(service)
    {
    }
}
