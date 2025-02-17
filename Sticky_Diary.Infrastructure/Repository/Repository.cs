using Microsoft.EntityFrameworkCore;
using Sticky_Dairy.Infrastructure.Data;
using Sticky_Dairy.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Sticky_Dairy.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly Sticky_Dairy_dbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(Sticky_Dairy_dbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T> GetByIdAsync(Guid id) => await _dbSet.FindAsync(id);

        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

        public async Task<T> FindAsync(Expression<Func<T, bool>> predicate) => await _dbSet.FirstOrDefaultAsync(predicate);

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
