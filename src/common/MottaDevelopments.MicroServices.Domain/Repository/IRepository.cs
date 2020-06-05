using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MottaDevelopments.MicroServices.Domain.Entities;
using MottaDevelopments.MicroServices.Domain.UnitOfWork;

namespace MottaDevelopments.MicroServices.Domain.Repository
{
    public interface IRepository<TEntity> where  TEntity : IEntity
    {
        IUnitOfWork UnitOfWork { get; }

        TEntity Add(TEntity entity);

        void AddRange(IEnumerable<TEntity> entities);
        
        void Remove(TEntity entity);

        void RemoveRange(IEnumerable<TEntity> entities);

        void Update(TEntity entity);
        
        void UpdateRange(IEnumerable<TEntity> entities);

        Task<TEntity> FindEntityAsync(Expression<Func<TEntity, bool>> expression);

        Task<IEnumerable<TEntity>> FindEntitiesAsync(Expression<Func<TEntity, bool>> expression);

        Task<IEnumerable<TEntity>> GetAllEntitiesAsync();
    }
}