using Backend.Database;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class DigitalDataRepository(DatabaseContext context) : CrudRepository<DigitalData>(context), IDigitalDataRepository
{
    public async Task<List<DigitalData>> FindByTagId(Guid id)
    {
        return await Entities
            .Where(e => e.TagId == id).ToListAsync();
    }

    public async Task<List<DigitalData>> FindByIdByTime(Guid id, DateTime from, DateTime to)
    {
        return await Entities
            .Where(e => e.TagId == id && e.Timestamp >= from && e.Timestamp <= to)
            .ToListAsync();    
    }

    public async Task<DigitalData?> FindLatestById(Guid id)
    {
        return await Entities.OrderByDescending(e => e.Timestamp)
            .Where(e => e.TagId == id).FirstOrDefaultAsync();
    }

    public async Task DeleteByTag(Guid id)
    {
        var entities = await Entities.Where(e => e.TagId == id).ToListAsync();
        if (entities.Count > 0)
        {
            Entities.RemoveRange(entities);
            await Context.SaveChangesAsync();
        }
    }
}