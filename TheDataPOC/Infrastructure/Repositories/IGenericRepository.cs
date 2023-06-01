﻿namespace Infrastructure.Repositories
{
    using System.Linq.Expressions;
    using Domain.Models;

    public interface IGenericRepository<TEntity> where TEntity : class
	{
        Task<TEntity> GetByIdAsync(int id);

        void Update(TEntity entity);

        Task AddAsync(TEntity entity);

        void Remove(TEntity entity);

        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);

        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>>? expression = null);

        Task AddRangeAsync(IEnumerable<TEntity> entities);

        Task<IList<string>> GetUserRolesAsync(int userId);

        Task<Role> GetRoleByNameAsync(string normalizedRoleName);

        Task<IList<User>> GetUsersInRoleAsync(int roleId);

        Task<TEntity> GetByIdAsync(params object[] idValues);
    }
}