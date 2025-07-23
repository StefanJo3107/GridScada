using Backend.Models;

namespace Backend.Hubs;

public interface ITagClient
{
    Task ReceiveAnalogData(AnalogData data);
    Task ReceiveDigitalData(DigitalData data);
}