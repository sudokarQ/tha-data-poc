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

        public Task AddAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AnyAsync(Expression<Predicate<TEntity>> predicate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> Get(Expression<Predicate<TEntity>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Remove(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Update(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}

