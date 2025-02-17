﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Sticky_Dairy.Application.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();     
        Task AddAsync(T entity);
        
        Task UpdateAsync(T entity);
        Task DeleteAsync(Guid id);
        Task<T> FindAsync(Expression<Func<T, bool>> predicate);
    }
}
