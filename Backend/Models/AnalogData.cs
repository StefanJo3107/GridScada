namespace Backend.Models;

public class AnalogData : IBaseEntity
{
    public AnalogData()
    {
    }

    public AnalogData(Guid id, AnalogInput? analogInput, double value, DateTime timestamp)
    {
        Id = id;
        AnalogInput = analogInput;
        Value = value;
        Timestamp = timestamp;
    }

    public AnalogInput? AnalogInput { get; set; }
    public double Value { get; set; }
    public DateTime Timestamp { get; set; }
    public Guid Id { get; set; }
}