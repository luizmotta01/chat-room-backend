using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MediatR;
using MongoDB.Driver;
using MottaDevelopments.MicroServices.Domain.Entities;
using MottaDevelopments.MicroServices.Infrastructure.MongoDb.Context;

namespace MottaDevelopments.MicroServices.Infrastructure.MongoDb
{
    public class MongoDbRepository<TEntity> : IMongoDbRepository<TEntity> where TEntity : class, IEntity
    {
        private readonly IMongoDbContextBase _context;
        private readonly IMediator _mediator;

        public MongoDbRepository(IMongoDbContextBase context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public IMongoDbContextBase Database => _context;

        private IMongoCollection<TEntity> GetCollection(string collectionName) => _context.Database.GetCollection<TEntity>(collectionName);
        
        private async Task DispatchDomainEvents(TEntity document) => await _mediator.SendDomainEventsAsync(document);

        private async Task DispatchDomainEvents(List<TEntity> documentsList) => await _mediator.SendDomainEventsAsync(documentsList);

        public async Task<TEntity> Add(string collectionName, TEntity document)
        {
            var collection = GetCollection(collectionName);

            await collection.InsertOneAsync(document);

            await DispatchDomainEvents(document);
            
            return document;
        }
        
        public async Task<IEnumerable<TEntity>> AddRange(string collectionName, IEnumerable<TEntity> documents)
        {
            var collection = GetCollection(collectionName);

            var documentsList = documents.ToList();

            await collection.InsertManyAsync(documentsList);

            await DispatchDomainEvents(documentsList);

             return documentsList;
        }
        
        public async Task<TEntity> Remove(string collectionName, TEntity document)
        {
            var collection = GetCollection(collectionName);

            await collection.DeleteOneAsync(doc => doc.Id == document.Id);

            await DispatchDomainEvents(document);

            return document;
        }

        public async Task<IEnumerable<TEntity>> RemoveRange(string collectionName, IEnumerable<TEntity> documents)
        {
            var collection = GetCollection(collectionName);

            var documentsList = documents.ToList();

            await collection.DeleteManyAsync(doc => documentsList.Contains(doc));

            await DispatchDomainEvents(documentsList);

            return documentsList;
        }

        public async Task<TEntity> Update(string collectionName, TEntity document)
        {
            var collection = GetCollection(collectionName);

            await collection.FindOneAndReplaceAsync(doc => doc.Id == document.Id, document);
            
            await DispatchDomainEvents(document);

            return document;
        }

        public async Task<IEnumerable<TEntity>> UpdateRange(string collectionName, IEnumerable<TEntity> documents)
        {
            var documentsList = documents.ToList();

            foreach (var documentFromList in documentsList) 
                await Update(collectionName, documentFromList);

            return documentsList;
        }

        public async Task<TEntity> FindEntityAsync(string collectionName, Expression<Func<TEntity, bool>> expression)
        {
            var collection = GetCollection(collectionName);
            
            var document = await (await collection.FindAsync<TEntity>(expression)).FirstOrDefaultAsync();

            await DispatchDomainEvents(document);

            return document;
        }

        public async Task<IEnumerable<TEntity>> FindEntitiesAsync(string collectionName, Expression<Func<TEntity, bool>> expression)
        {
            var collection = GetCollection(collectionName);

            var documentsList = await (await collection.FindAsync<TEntity>(expression)).ToListAsync();

            await DispatchDomainEvents(documentsList);

            return documentsList;
        }

        public async Task<IEnumerable<TEntity>> GetAllEntitiesAsync(string collectionName)
        {
            var collection = GetCollection(collectionName);

            var documents = await collection.FindAsync<TEntity>(collectionName);

            var documentsList = documents.ToList();

            await DispatchDomainEvents(documentsList);
            
            return documentsList;

        }
    }
}