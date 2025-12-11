using GoodFish.BLL.Models;
using GoodFish.BLL.Services;
using GoodFish.DAL.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GoodFish.API.Controllers.Default
{
    [ApiController]
    public class CRUDGenericController<TEntity>(IBaseCRUDEntityService<TEntity> service) : ControllerBase where TEntity : class, IEntity
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken token)
        {
            List<TEntity> entityResults = await service.GetAllAsync(token);
            return Ok(entityResults);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(DefaultErrorMessage))]
        public async Task<IActionResult> GetById(
            [FromRoute] int id,
            CancellationToken token)
        {
            TEntity entityResult = await service.GetByIdAsync(id, token);
            return Ok(entityResult);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create(
            [FromBody] TEntity entity,
            CancellationToken token)
        {
            int createdID = await service.CreateAsync(entity, token);
            return CreatedAtAction(nameof(GetById), new { id = createdID }, null);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(DefaultErrorMessage))]
        public async Task<IActionResult> Update(
            [FromBody] TEntity entity,
            CancellationToken token)
        {
            await service.UpdateAsync(entity, token);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(DefaultErrorMessage))]
        public async Task<IActionResult> Delete(
            [FromRoute] int id,
            CancellationToken token)
        {
            await service.DeleteAsync(id, token);
            return NoContent();
        }
    }
}
