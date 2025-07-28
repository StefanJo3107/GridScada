using Backend.Database;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class DigitalDataRepository(DatabaseContext context)
    : CrudRepository<DigitalData>(context), IDigitalDataRepository
{
    public async Task<List<DigitalData>> FindByTagId(Guid id)
    {
        await Global.Semaphore.WaitAsync();

        try
        {
            await Task.Delay(1);
            return await Entities
                .Where(e => e.DigitalInput.Id.ToString().ToLower().Equals(id.ToString().ToLower())).ToListAsync();
        }
        finally
        {
            Global.Semaphore.Release();
        }
    }

    public async Task<List<DigitalData>> FindByTagIdByTime(Guid id, DateTime from, DateTime to)
    {
        await Global.Semaphore.WaitAsync();

        try
        {
            await Task.Delay(1);
            return await Entities
                .Where(e => e.DigitalInput.Id.ToString().ToLower().Equals(id.ToString().ToLower()) &&
                            e.Timestamp >= from &&
                            e.Timestamp <= to)
                .ToListAsync();
        }
        finally
        {
            Global.Semaphore.Release();
        }
    }

    public async Task<DigitalData?> FindLatestByTagId(Guid id)
    {
        await Global.Semaphore.WaitAsync();

        try
        {
            await Task.Delay(1);
            return await Entities.OrderByDescending(e => e.Timestamp)
                .Where(e => e.DigitalInput.Id.ToString().ToLower().Equals(id.ToString().ToLower()))
                .FirstOrDefaultAsync();
        }
        finally
        {
            Global.Semaphore.Release();
        }
    }

    public async Task DeleteByTagId(Guid id)
    {
        await Global.Semaphore.WaitAsync();

        try
        {
            await Task.Delay(1);
            var entities = await Entities
                .Where(e => e.DigitalInput.Id.ToString().ToLower().Equals(id.ToString().ToLower())).ToListAsync();
            if (entities.Count > 0)
            {
                Entities.RemoveRange(entities);
                await Context.SaveChangesAsync();
            }
        }
        finally
        {
            Global.Semaphore.Release();
        }
    }
}