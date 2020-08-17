using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MottaDevelopments.MicroServices.Domain.Entities;
using MottaDevelopments.MicroServices.Domain.Repository;
using MottaDevelopments.MicroServices.Domain.UnitOfWork;
using MottaDevelopments.MicroServices.Infrastructure.EfCore.Context;

namespace MottaDevelopments.MicroServices.Infrastructure.EfCore.Repository
{
    public class EfCoreRepository<TEntity> : IEfCoreRepository<TEntity> where TEntity : class, IEntity
    {
        private readonly DbContextBase _context;

        public EfCoreRepository(DbContextBase context) => _context = context;

        public IUnitOfWork UnitOfWork => _context;
        
        public TEntity Add(TEntity entity) => _context.Set<TEntity>().Add(entity).Entity;

        public void AddRange(IEnumerable<TEntity> entities) => _context.Set<TEntity>().AddRange(entities);

        public void Remove(TEntity entity) => _context.Set<TEntity>().Remove(entity);

        public void RemoveRange(IEnumerable<TEntity> entities) => _context.Set<TEntity>().RemoveRange(entities);

        public void Update(TEntity entity) => _context.Set<TEntity>().Update(entity);

        public void UpdateRange(IEnumerable<TEntity> entities) => _context.Set<TEntity>().UpdateRange(entities);

        public async Task<TEntity> FindEntityAsync(Expression<Func<TEntity, bool>> expression) => await _context.Set<TEntity>().FirstOrDefaultAsync(expression);

        public async Task<IEnumerable<TEntity>> FindEntitiesAsync(Expression<Func<TEntity, bool>> expression) => await _context.Set<TEntity>().Where(expression).ToListAsync();

        public async Task<IEnumerable<TEntity>> GetAllEntitiesAsync() => await _context.Set<TEntity>().ToListAsync();
    }
}