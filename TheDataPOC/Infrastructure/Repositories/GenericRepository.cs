namespace Infrastructure.Repositories
{
    using System.Linq.Expressions;

    using Infrastructure.Database;

    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity: class
	{
        protected readonly ApplicationContext context;

        public GenericRepository(ApplicationContext context)
        {
            this.context = context;
        }

        Task IGenericRepository<TEntity>.AddAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        Task IGenericRepository<TEntity>.AddRangeAsync(IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        Task<bool> IGenericRepository<TEntity>.AnyAsync(Expression<Predicate<TEntity>> predicate)
        {
            throw new NotImplementedException();
        }

        IEnumerable<TEntity> IGenericRepository<TEntity>.Get(Expression<Predicate<TEntity>> predicate)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<TEntity>> IGenericRepository<TEntity>.GetAllAsync()
        {
            throw new NotImplementedException();
        }

        Task<TEntity> IGenericRepository<TEntity>.GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        void IGenericRepository<TEntity>.Remove(TEntity entity)
        {
            throw new NotImplementedException();
        }

        void IGenericRepository<TEntity>.Update(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}

