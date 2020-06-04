﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MottaDevelopments.MicroServices.Domain.Entities;
using MottaDevelopments.MicroServices.Domain.UnitOfWork;

namespace MottaDevelopments.MicroServices.Domain.Repository
{
    public interface IRepository<TEntity> where  TEntity : IEntity
    {
        IUnitOfWork UnitOfWork { get; }

        TEntity Add(TEntity entity);

        void AddRange(IEnumerable<TEntity>entities);

        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<TEntity>GetByIdAsync(Guid id);

        void Remove(TEntity entity);

        void RemoveRange(IEnumerable<TEntity>entities);

        void Update(TEntity entity);
    }
}