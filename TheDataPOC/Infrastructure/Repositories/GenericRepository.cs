namespace Infrastructure.Repositories
{
    using System.Linq;
    using System.Linq.Expressions;
    
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

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await dbSet.AddRangeAsync(entities);
        }

        public async Task<bool> AnyAsync()
        {
            return await dbSet.AnyAsync();
        }

        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>>? expression = null)
        {
            if (expression is null)
            {
                return dbSet.AsQueryable();
            }

            return dbSet.Where(expression).AsQueryable();
        }

        public void Remove(TEntity entity)
        {
            dbSet.Remove(entity);
        }

        public void Update(TEntity entity)
        {
            dbSet.Update(entity);
        }
    }
}

