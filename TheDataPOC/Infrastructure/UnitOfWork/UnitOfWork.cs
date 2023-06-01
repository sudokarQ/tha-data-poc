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

        IDbContextTransaction IUnitOfWork.BeginTransaction(IsolationLevel isolationLevel)
        {
            throw new NotImplementedException();
        }

        void IDisposable.Dispose()
        {
            throw new NotImplementedException();
        }

        IGenericRepository<TEntity> IUnitOfWork.GetRepository<TEntity>()
        {
            throw new NotImplementedException();
        }

        Task IUnitOfWork.SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}