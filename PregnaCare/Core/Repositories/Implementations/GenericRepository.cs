﻿using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PregnaCare.Core.Repositories.Interfaces;
using PregnaCare.Infrastructure.Data;

namespace PregnaCare.Core.Repositories.Implementations
{
    /// <summary>
    /// GenericRepository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public class GenericRepository<T, TKey> : IGenericRepository<T, TKey> where T : class
    {
        private readonly PregnaCareAppDbContext _pregnaCareAppDbContext;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(PregnaCareAppDbContext pregnaCareAppDbContext)
        {
            _pregnaCareAppDbContext = pregnaCareAppDbContext;
            _dbSet = _pregnaCareAppDbContext.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            _ = await _dbSet.AddAsync(entity);
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<T>> FindWithIncludesAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(TKey id)
        {
            return await _dbSet.FindAsync(id);
        }

        public void Remove(T entity)
        {
            _ = _dbSet.Remove(entity);
        }

        public void Update(T entity)
        {
            _ = _dbSet.Update(entity);
        }
    }
}
