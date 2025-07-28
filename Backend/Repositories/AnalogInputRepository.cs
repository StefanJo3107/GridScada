using Backend.Database;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class AnalogInputRepository(DatabaseContext context)
    : CrudRepository<AnalogInput>(context), IAnalogInputRepository
{
    public async Task<AnalogInput?> FindByIdWithAlarmsAndUsers(Guid id)
    {
        await Global.Semaphore.WaitAsync();

        try
        {
            await Task.Delay(1);
            return await Entities.Include(e => e.Alarms)
                .Include(u => u.Users)
                .FirstOrDefaultAsync(e => e.Id.ToString().ToLower().Equals(id.ToString().ToLower()));
        }
        finally
        {
            Global.Semaphore.Release();
        }
    }

    public async Task DeleteAlarms(Guid tagId)
    {
        await Global.Semaphore.WaitAsync();

        try
        {
            await Task.Delay(1);
            var tag = await Context.AnalogInput.Include(t => t.Alarms)
                .FirstOrDefaultAsync(t => t.Id.ToString().ToLower().Equals(tagId.ToString().ToLower()));
            if (tag != null)
            {
                tag.Alarms.Clear();
                await Context.SaveChangesAsync();
            }
        }
        finally
        {
            Global.Semaphore.Release();
        }
    }
}