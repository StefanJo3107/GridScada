using Backend.Database;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class AlarmAlertRepository(DatabaseContext context) : CrudRepository<AlarmAlert>(context), IAlarmAlertRepository
{
    public async Task DeleteByAlarmId(Guid id)
    {
        var entityToDelete = await ReadAll();
        await Global.Semaphore.WaitAsync();
        try
        {
            foreach (var alarmAlert in entityToDelete)
            {
                if (alarmAlert.AlarmId != id) continue;
                Context.Remove(alarmAlert);
                await Context.SaveChangesAsync();
            }

            await Task.Delay(1);
        }
        finally
        {
            Global.Semaphore.Release();
        }
    }

    public async Task<IEnumerable<AlarmAlert>> FindByIdByTime(Guid id, DateTime from, DateTime to)
    {
        await Global.Semaphore.WaitAsync();
        try
        {
            await Task.Delay(1);
            return await Entities
                .Where(e => e.AlarmId.ToString().ToLower().Equals(id.ToString().ToLower()) && e.Timestamp >= from &&
                            e.Timestamp <= to)
                .ToListAsync();
        }
        finally
        {
            Global.Semaphore.Release();
        }
    }

    public async Task<List<AlarmAlert>> FindByAlarmId(Guid id)
    {
        await Global.Semaphore.WaitAsync();
        try
        {
            await Task.Delay(1);
            return await Entities
                .Where(e => e.AlarmId.ToString().ToLower().Equals(id.ToString().ToLower())).ToListAsync();
        }
        finally
        {
            Global.Semaphore.Release();
        }
    }
}