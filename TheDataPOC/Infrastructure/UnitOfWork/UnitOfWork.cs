namespace Infrastructure.UnitOfWork
{
    using System.Data;

    using Domain.Models;

    using Infrastructure.Database;
    using Infrastructure.Repositories;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage;

    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationContext context;

        private bool disposed = false;

        private readonly Dictionary<Type, object> repositories;

        private readonly Dictionary<Type, Type> registeredRepositories;
        
        public IUserRepository UserRepository => (IUserRepository)GetRepository<User>();

        public IRoleRepository RoleRepository => (IRoleRepository)GetRepository<Role>();


        public UnitOfWork(ApplicationContext context)
		{
            this.context = context;
            this.repositories = new Dictionary<Type, object>();
            this.registeredRepositories = new Dictionary<Type, Type>();

            RegisterRepository<User, UserRepository>();
            RegisterRepository<Role, RoleRepository>();
		}

        public IGenericRepository<TEntity> GetRepository<TEntity>()
            where TEntity : class
        {
            var entityType = typeof(TEntity);

            if(repositories.TryGetValue(entityType, out var repositoryObject))
            {
                return (IGenericRepository<TEntity>)repositoryObject;
            }

            var createdRepository = CreateRepository<TEntity>();
            repositories.Add(entityType, createdRepository);

            return createdRepository;
        }

        protected void RegisterRepository<TEntity, TRepository>()
            where TEntity : class
            where TRepository : IGenericRepository<TEntity>
        {
            registeredRepositories.Add(typeof(TEntity), typeof(TRepository));
        }

        private IGenericRepository<TEntity> CreateRepository<TEntity>()
            where TEntity : class 
        {
            var entityType = typeof(TEntity);

            if(!registeredRepositories.TryGetValue(entityType, out var repositoryType))
            {
                return new GenericRepository<TEntity>(context);
            }

            var customRepository = Activator.CreateInstance(repositoryType, context);

            return (IGenericRepository<TEntity>)customRepository;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel)
        {
            return await context.Database.BeginTransactionAsync(isolationLevel);
        }

        public async Task<int> SaveAsync()
        {
            return await context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            
            disposed = true;
        }
    }
}