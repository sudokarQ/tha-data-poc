namespace Infrastructure.UnitOfWork
{
    using System.Data;

    using Infrastructure.Repositories;

    using Microsoft.EntityFrameworkCore.Storage;

    public class UnitOfWork : IUnitOfWork
	{
		public UnitOfWork()
		{
		}

        public IDbContextTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity: class
        {
            throw new NotImplementedException();
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}