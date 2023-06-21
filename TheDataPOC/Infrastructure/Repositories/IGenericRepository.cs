namespace Infrastructure.Repositories
{
    using System.Linq.Expressions;

    public interface IGenericRepository<TEntity> where TEntity : class
	{
        void Update(TEntity entity);

        void Remove(TEntity entity);

        Task<bool> AnyAsync();

        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>>? expression = null);

        Task AddRangeAsync(IEnumerable<TEntity> entities);
    }
}

