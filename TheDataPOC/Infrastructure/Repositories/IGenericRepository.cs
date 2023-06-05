namespace Infrastructure.Repositories
{
    using System.Linq.Expressions;

    public interface IGenericRepository<TEntity> where TEntity : class
	{
        Task<TEntity> GetByIdAsync(Guid id);

        Task<IEnumerable<TEntity>> GetAllAsync();

        void Update(TEntity entity);

        Task AddAsync(TEntity entity);

        void Remove(TEntity entity);

        Task<bool> AnyAsync(Expression<Predicate<TEntity>> predicate);

        IEnumerable<TEntity> Get(Expression<Predicate<TEntity>> predicate);

        Task AddRangeAsync(IEnumerable<TEntity> entities);
    }
}

