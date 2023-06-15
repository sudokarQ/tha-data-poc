namespace Infrastructure.UnitOfWork
{
    using System.Data;

    using Infrastructure.Repositories;

    using Microsoft.EntityFrameworkCore.Storage;

    public interface IUnitOfWork : IDisposable
	{
        public IUserRepository UserRepository { get; }

        public IRoleRepository RoleRepository { get; }
        
        public Task<int> SaveAsync();

        IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : class;

        public Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel);
    }
}

