using GoodFish.DAL.Models.Interfaces;

namespace GoodFish.BLL.Services
{
    public interface IBaseCRUDEntityService<TEntity> where TEntity : class, IEntity
    {
        Task<List<TEntity>> GetAllAsync(CancellationToken token);
        Task<TEntity> GetByIdAsync(int id, CancellationToken token);
        Task<int> CreateAsync(TEntity entity, CancellationToken token);
        Task UpdateAsync(TEntity entity, CancellationToken token);
        Task DeleteAsync(int id, CancellationToken token);
    }
}
