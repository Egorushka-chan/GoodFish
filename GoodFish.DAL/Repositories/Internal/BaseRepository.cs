using GoodFish.DAL.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GoodFish.DAL.Repositories.Internal
{
    internal class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, IEntity
    {
        protected readonly MainDbContext _dbContext;

        public BaseRepository(MainDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            return _dbContext.Set<TEntity>()
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public virtual Task<TEntity?> GetByIdAsync(long id, CancellationToken cancellationToken)
        {
            return _dbContext.Set<TEntity>()
                .Where(x => x.ID == id)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);
        }

        public virtual async Task CreateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            _dbContext.Set<TEntity>().Add(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task<bool> RemoveByIdAsync(long id, CancellationToken cancellationToken)
        {
            int count = await _dbContext.Set<TEntity>()
                .Where(ent => ent.ID == id)
                .ExecuteDeleteAsync(cancellationToken);
            return count > 0;
        }

        public virtual async Task RemoveAsync(TEntity entity, CancellationToken cancellationToken)
        {
            _dbContext.Set<TEntity>().Remove(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            _dbContext.Set<TEntity>().Update(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
