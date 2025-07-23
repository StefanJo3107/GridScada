using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Database;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class DigitalDataRepository(DatabaseContext context) : CrudRepository<DigitalData>(context), IDigitalDataRepository
{
    public async Task<List<DigitalData>> FindByTagId(Guid id)
    {
        return await Entities
            .Where(e => e.DigitalInput.Id == id).ToListAsync();
    }

    public async Task<List<DigitalData>> FindByTagIdByTime(Guid id, DateTime from, DateTime to)
    {
        return await Entities
            .Where(e => e.DigitalInput.Id == id && e.Timestamp >= from && e.Timestamp <= to)
            .ToListAsync();    
    }

    public async Task<DigitalData?> FindLatestByTagId(Guid id)
    {
        return await Entities.OrderByDescending(e => e.Timestamp)
            .Where(e => e.DigitalInput.Id == id).FirstOrDefaultAsync();
    }

    public async Task DeleteByTagId(Guid id)
    {
        var entities = await Entities.Where(e => e.DigitalInput.Id == id).ToListAsync();
        if (entities.Count > 0)
        {
            Entities.RemoveRange(entities);
            await Context.SaveChangesAsync();
        }
    }
}