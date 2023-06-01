namespace Infrastructure.UnitOfWork
{
    using System.Data;

    using Infrastructure.Repositories;

    using Microsoft.EntityFrameworkCore.Storage;

    public interface IUnitOfWork : IDisposable
	{
        Task SaveChangesAsync();

        IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : class;

        public IDbContextTransaction BeginTransaction(IsolationLevel isolationLevel);
    }
}

