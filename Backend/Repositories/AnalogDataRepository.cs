using Backend.Database;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class AnalogDataRepository(DatabaseContext context) : CrudRepository<AnalogData>(context), IAnalogDataRepository
{
    public async Task<List<AnalogData>> FindByTagId(Guid id)
    {
        return await Entities
            .Where(e => e.TagId == id).ToListAsync();
    }

    public async Task<List<AnalogData>> FindByIdByTime(Guid id, DateTime from, DateTime to)
    {
        return await Entities
            .Where(e => e.TagId == id && e.Timestamp >= from && e.Timestamp <= to)
            .ToListAsync();
        
    }

    public async Task<AnalogData?> FindLatestById(Guid id)
    {
        return await Entities.OrderByDescending(e => e.Timestamp)
            .Where(e => e.TagId == id).FirstOrDefaultAsync();
    }

    public async Task DeleteByTagId(Guid id)
    {
        var entities = await Entities.Where(e => e.TagId == id).ToListAsync();
        if (entities.Count > 0)
        {
            Entities.RemoveRange(entities);
            await Context.SaveChangesAsync();
        }
    }
}