using GoodFish.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace GoodFish.API.Controllers.ETL
{
    [ApiController]
    [Route("api/etl/customer")]
    public class CustomerETLController(IETLService service) : ControllerBase
    {
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> RunCustomerETL(
            IFormFile file)
        {
            await service.ProcessCustomersAsync(file);
            return Ok();
        }
    }
}
