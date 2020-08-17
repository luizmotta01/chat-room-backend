using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;
using MottaDevelopments.MicroServices.Domain.Entities;
using MottaDevelopments.MicroServices.Infrastructure.MongoDb.Context;

namespace MottaDevelopments.MicroServices.Infrastructure.MongoDb
{
    public interface IMongoDbRepository<TEntity> where TEntity : class, IEntity
    {
        IMongoDbContextBase Database { get; }

        Task<TEntity> Add(string collectionName, TEntity document);

        Task<IEnumerable<TEntity>> AddRange(string collectionName, IEnumerable<TEntity> documents);

        Task<TEntity> Remove(string collectionName, TEntity document);

        Task<IEnumerable<TEntity>> RemoveRange(string collectionName, IEnumerable<TEntity> documents);

        Task<TEntity> Update(string collectionName, TEntity document);

        Task<IEnumerable<TEntity>> UpdateRange(string collectionName, IEnumerable<TEntity> documents);

        Task<TEntity> FindEntityAsync(string collectionName, Expression<Func<TEntity, bool>> expression);

        Task<IEnumerable<TEntity>> FindEntitiesAsync(string collectionName, Expression<Func<TEntity, bool>> expression);

        Task<IEnumerable<TEntity>> GetAllEntitiesAsync(string collectionName);

    }
}