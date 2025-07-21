using System;
using System.Threading.Tasks;
using Backend.Database;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class AnalogInputRepository(DatabaseContext context) : CrudRepository<AnalogInput>(context), IAnalogInputRepository
{
    public async Task<AnalogInput?> FindByIdWithAlarmsAndUsers(Guid id)
    {
        return await Entities.Include(e => e.Alarms)
            .Include(u => u.Users).FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task DeleteAlarms(Guid tagId)
    {
        var tag = await Context.AnalogInput.Include(t => t.Alarms)
            .FirstOrDefaultAsync(t => t.Id == tagId);
        if (tag != null)
        {
            tag.Alarms.Clear();
            await Context.SaveChangesAsync();
        }
    }
}