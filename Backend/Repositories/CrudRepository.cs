using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Database;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class CrudRepository<T>(DatabaseContext context) : ICrudRepository<T>
    where T : class, IBaseEntity
{
    protected readonly DatabaseContext Context = context;
    protected readonly DbSet<T> Entities = context.Set<T>();

    public async Task<IEnumerable<T>> ReadAll()
    {
        return await Entities.ToListAsync();
    }

    public async Task<T?> Read(Guid id)
    {
        return await Entities.FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<T> Create(T entity)
    {
        await Entities.AddAsync(entity);
        await Context.SaveChangesAsync();
        return entity;
    }

    public async Task<T?> Update(T entity)
    {
        var entityToUpdate = await Entities.FirstOrDefaultAsync(e => e.Id == entity.Id);
        if (entityToUpdate == null) return entityToUpdate;
        Context.Entry(entityToUpdate).CurrentValues.SetValues(entity);
        await Context.SaveChangesAsync();

        return entityToUpdate;
    }

    public async Task<T?> Delete(Guid id)
    {
        var entityToDelete = await Entities.FirstOrDefaultAsync(e => e.Id == id);
        if (entityToDelete == null) return entityToDelete;
        Context.Remove(entityToDelete);
        await Context.SaveChangesAsync();
        return entityToDelete;
    }
}