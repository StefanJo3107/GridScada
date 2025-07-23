using Backend.Database;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class AnalogDataRepository(DatabaseContext context) : CrudRepository<AnalogData>(context), IAnalogDataRepository
{
    public async Task<List<AnalogData>> FindByTagId(Guid id)
    {
        return await Entities
            .Where(e => e.AnalogInput.Id == id).ToListAsync();
    }

    public async Task<List<AnalogData>> FindByTagIdByTime(Guid id, DateTime from, DateTime to)
    {
        return await Entities
            .Where(e => e.AnalogInput.Id == id && e.Timestamp >= from && e.Timestamp <= to)
            .ToListAsync();
    }

    public async Task<AnalogData?> FindLatestByTagId(Guid id)
    {
        return await Entities.OrderByDescending(e => e.Timestamp)
            .Where(e => e.AnalogInput.Id == id).FirstOrDefaultAsync();
    }

    public async Task DeleteByTagId(Guid id)
    {
        var entities = await Entities.Where(e => e.AnalogInput.Id == id).ToListAsync();
        if (entities.Count > 0)
        {
            Entities.RemoveRange(entities);
            await Context.SaveChangesAsync();
        }
    }
}