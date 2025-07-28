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
        await Global.Semaphore.WaitAsync();

        try
        {
            await Task.Delay(1);
            return await Entities.ToListAsync();
        }
        finally
        {
            Global.Semaphore.Release();
        }
    }

    public async Task<T?> Read(Guid id)
    {
        await Global.Semaphore.WaitAsync();

        try
        {
            await Task.Delay(1);
            return await Entities.FirstOrDefaultAsync(e => e.Id.ToString().ToLower().Equals(id.ToString().ToLower()));
        }
        finally
        {
            Global.Semaphore.Release();
        }
    }

    public async Task<T> Create(T entity)
    {
        await Global.Semaphore.WaitAsync();

        try
        {
            await Task.Delay(1);
            await Entities.AddAsync(entity);
            await Context.SaveChangesAsync();
            return entity;
        }
        finally
        {
            Global.Semaphore.Release();
        }
    }

    public async Task<T?> Update(T entity)
    {
        await Global.Semaphore.WaitAsync();

        try
        {
            await Task.Delay(1);
            var entityToUpdate =
                await Entities.FirstOrDefaultAsync(
                    e => e.Id.ToString().ToLower().Equals(entity.Id.ToString().ToLower()));
            if (entityToUpdate == null) return entityToUpdate;
            Context.Entry(entityToUpdate).CurrentValues.SetValues(entity);
            await Context.SaveChangesAsync();

            return entityToUpdate;
        }
        finally
        {
            Global.Semaphore.Release();
        }
    }

    public async Task<T?> Delete(Guid id)
    {
        await Global.Semaphore.WaitAsync();

        try
        {
            await Task.Delay(1);
            var entityToDelete =
                await Entities.FirstOrDefaultAsync(e => e.Id.ToString().ToLower().Equals(id.ToString().ToLower()));
            if (entityToDelete == null) return entityToDelete;
            Context.Remove(entityToDelete);
            await Context.SaveChangesAsync();
            return entityToDelete;
        }
        finally
        {
            Global.Semaphore.Release();
        }
    }
}