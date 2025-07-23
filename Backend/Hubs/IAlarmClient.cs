using Backend.Models;

namespace Backend.Hubs;

public interface IAlarmClient
{
    Task ReceiveAlarmData(AlarmReport data);
}