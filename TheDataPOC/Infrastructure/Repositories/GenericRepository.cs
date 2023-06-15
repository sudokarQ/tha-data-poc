﻿namespace Infrastructure.Repositories
{
    using System.Linq;
    using System.Linq.Expressions;

    using Domain.Models;
    
    using Infrastructure.Database;

    using Microsoft.EntityFrameworkCore;

    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected readonly ApplicationContext context;

        protected readonly DbSet<TEntity> dbSet;
        
        public GenericRepository(ApplicationContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public async Task AddAsync(TEntity entity)
        {
            await dbSet.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await dbSet.AddRangeAsync(entities);
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await dbSet.AnyAsync(predicate);
        }

        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>>? expression = null)
        {
            if (expression is null)
            {
                return dbSet.AsQueryable();
            }

            return dbSet.Where(expression).AsQueryable();
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await dbSet.FindAsync(id);
        }

        public void Remove(TEntity entity)
        {
            dbSet.Remove(entity);
        }

        public void Update(TEntity entity)
        {
            dbSet.Update(entity);
        }

        public async Task<TEntity> GetByIdAsync(params object[] idValues)
        {
            return await context.Set<TEntity>().FindAsync(idValues);
        }
    }
}

