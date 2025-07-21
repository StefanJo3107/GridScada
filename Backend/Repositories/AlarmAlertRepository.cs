using Backend.Database;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class AlarmAlertRepository(DatabaseContext context) : CrudRepository<AlarmAlert>(context), IAlarmAlertRepository
{
    public async Task DeleteByAlarmId(Guid id)
    {
        var entityToDelete = await ReadAll();
        foreach (var alarmAlert in entityToDelete)
        {
            if (alarmAlert.AlarmId != id) continue;
            Context.Remove(alarmAlert);
            await Context.SaveChangesAsync();
        }

    }

    public async Task<IEnumerable<AlarmAlert>> FindByIdByTime(Guid id, DateTime from, DateTime to)
    {
        return await Entities
            .Where(e => e.AlarmId == id && e.Timestamp >= from && e.Timestamp <= to)
            .ToListAsync();    }

    public async Task<List<AlarmAlert>> FindByAlarmId(Guid id)
    {
        return await Entities
            .Where(e => e.AlarmId == id).ToListAsync();    
    }
}