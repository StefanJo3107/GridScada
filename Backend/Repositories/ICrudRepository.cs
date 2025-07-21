using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Models;

namespace Backend.Repositories;

public interface ICrudRepository<T> where T : class, IBaseEntity
{
    Task<IEnumerable<T>> ReadAll();
    Task<T?> Read(Guid id);
    Task<T> Create(T entity);
    Task<T?> Update(T entity);
    Task<T?> Delete(Guid id);
}