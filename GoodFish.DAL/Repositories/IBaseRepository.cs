using GoodFish.DAL.Models.Interfaces;

namespace GoodFish.DAL.Repositories
{
    public interface IBaseRepository<TEntity> where TEntity : class, IEntity
    {
        Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken);
        Task<TEntity?> GetByIdAsync(long id, CancellationToken cancellationToken);
        Task CreateAsync(TEntity entity, CancellationToken cancellationToken);
        Task<bool> RemoveByIdAsync(long id, CancellationToken cancellationToken);
        Task RemoveAsync(TEntity entity, CancellationToken cancellationToken);
        Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);
    }
}
