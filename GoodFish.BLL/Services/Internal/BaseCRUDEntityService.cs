using GoodFish.BLL.Models;
using GoodFish.DAL.Models.Interfaces;
using GoodFish.DAL.Repositories;
using Microsoft.Extensions.Logging;

namespace GoodFish.BLL.Services.Internal
{
    internal class BaseCRUDEntityService<TEntity>(IBaseRepository<TEntity> repository, ILogger<BaseCRUDEntityService<TEntity>> logger) 
        : IBaseCRUDEntityService<TEntity> where TEntity : class, IEntity
    {
        public async Task<List<TEntity>> GetAllAsync(CancellationToken token)
        {
            logger.LogDebug("Начато получение всех entities типа {EntityType}", TEntity.GetDisplayName());
            var entities = await repository.GetAllAsync(token);

            logger.LogInformation("Получено {Count} entities типа {EntityType}", entities.Count, TEntity.GetDisplayName());
            return entities;
        }

        public async Task<TEntity> GetByIdAsync(int id, CancellationToken token)
        {
            logger.LogDebug("Начато получение entity типа {EntityType} с ID {EntityId}", TEntity.GetDisplayName(), id);
            TEntity? entity = await repository.GetByIdAsync(id, token);

            if(entity is null)
            {
                logger.LogInformation("Entity типа {EntityType} с ID {EntityId} не найдена", TEntity.GetDisplayName(), id);
                throw new EntityNotFoundException($"Entity типа {TEntity.GetDisplayName()} с ID {id} не найдена.");
            }

            return entity;
        }

        public async Task<int> CreateAsync(TEntity entity, CancellationToken token)
        {
            logger.LogDebug("Начато создание entity типа {EntityType}", TEntity.GetDisplayName());
            await repository.CreateAsync(entity, token);

            logger.LogInformation("Создана entity типа {EntityType} с ID {EntityId}", TEntity.GetDisplayName(), entity.ID);
            return entity.ID;
        }

        public async Task DeleteAsync(int id, CancellationToken token)
        {
            logger.LogDebug("Начато удаление entity типа {EntityType} с ID {EntityId}", TEntity.GetDisplayName(), id);
            bool found = await repository.RemoveByIdAsync(id, token);
            if(found is false)
            {
                logger.LogInformation("Попытка удаления entity типа {EntityType} с ID {EntityId}, которая не существует", TEntity.GetDisplayName(), id);
                throw new EntityNotFoundException($"Entity типа {TEntity.GetDisplayName()} с ID {id} не найдена для удаления.");
            }
            logger.LogInformation("Удалена entity типа {EntityType} с ID {EntityId}", TEntity.GetDisplayName(), id);
        }

        public async Task UpdateAsync(TEntity entity, CancellationToken token)
        {
            TEntity? dbEntity = await repository.GetByIdAsync(entity.ID, token);
            if(dbEntity is null)
            {
                logger.LogInformation("Попытка обновления entity типа {EntityType} с ID {EntityId}, которая не существует", TEntity.GetDisplayName(), entity.ID);
                throw new EntityNotFoundException($"Entity типа {TEntity.GetDisplayName()} с ID {entity.ID} не найдена для обновления.");
            }

            logger.LogDebug("Начато обновление entity типа {EntityType} с ID {EntityId}", TEntity.GetDisplayName(), entity.ID);
            await repository.UpdateAsync(entity, token);
            logger.LogInformation("Обновлена entity типа {EntityType} с ID {EntityId}", TEntity.GetDisplayName(), entity.ID);
        }
    }
}
