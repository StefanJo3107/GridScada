namespace Backend.Models;

public class DigitalData : IBaseEntity
{
    public DigitalData()
    {
    }

    public DigitalData(Guid id, DigitalInput? digitalInput, double value, DateTime timestamp, Guid tagId)
    {
        Id = id;
        DigitalInput = digitalInput;
        Value = value;
        Timestamp = timestamp;
    }

    public DigitalInput? DigitalInput { get; set; }
    public double Value { get; set; }
    public DateTime Timestamp { get; set; }
    public Guid Id { get; set; }
}