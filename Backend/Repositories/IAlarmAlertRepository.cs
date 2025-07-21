using Backend.Models;

namespace Backend.Repositories;

public interface IAlarmAlertRepository : ICrudRepository<AlarmAlert>
{
    Task DeleteByAlarmId(Guid id);
    Task<IEnumerable<AlarmAlert>> FindByIdByTime(Guid id, DateTime from, DateTime to);
    Task<List<AlarmAlert>> FindByAlarmId(Guid id);
}