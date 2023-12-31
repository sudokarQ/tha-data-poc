﻿namespace Infrastructure.UnitOfWork
{
    using System.Data;

    using Infrastructure.Database;
    using Infrastructure.Repositories;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage;

    public class UnitOfWork : IDisposable, IUnitOfWork
	{
        private ApplicationContext context;

        private bool disposed = false;

        public UnitOfWork(ApplicationContext context)
		{
            this.context = context;
		}

        public IGenericRepository<T> GetRepository<T>() where T : class
        {
            return new GenericRepository<T>(context);
        }

        public IDbContextTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            return context.Database.BeginTransaction(isolationLevel);
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