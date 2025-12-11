using Microsoft.AspNetCore.Http;

namespace GoodFish.BLL.Services
{
    public interface IETLService
    {
        public Task ProcessCustomersAsync(IFormFile file);
    }
}
