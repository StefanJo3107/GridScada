using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Backend.Hubs;

[Authorize]
public class AlarmHub : Hub<IAlarmClient>
{
    public AlarmHub()
    {

    }

    public async Task SendAlarmDataToAllClients(AlarmReport data)
    {
        await Clients.All.ReceiveAlarmData(data);
    }
}