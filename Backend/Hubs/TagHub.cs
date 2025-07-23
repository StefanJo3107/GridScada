using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Backend.Hubs;

[Authorize]
public class TagHub : Hub<ITagClient>
{
    public TagHub()
    {

    }

    public async Task SendAnalogDataToAllClients(AnalogData data)
    {
        await Clients.All.ReceiveAnalogData(data);
    }

    public async Task SendDigitalDataToAllClients(DigitalData data)
    {
        await Clients.All.ReceiveDigitalData(data);
    }
}
