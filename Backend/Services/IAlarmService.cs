using Backend.Models;

namespace Backend.Services;

public interface IAlarmService
{
    Task MakeAlarm(Guid userId, Alarm alarm);
    Task DeleteAlarm(Guid userId, Guid alarmId, Guid tagId);
    Task<List<AlarmReport>> GetAllAlarmsByTime(Guid userId, DateTime timeFrom, DateTime timeTo);
    Task<List<AlarmReport>> GetAllAlarmsByPriority(Guid userId, AlarmPriority priority);
}