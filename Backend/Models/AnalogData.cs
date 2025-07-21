namespace Backend.Models;

public class AnalogData : IBaseEntity
{
    public Guid Id { get; set; }
    public AnalogInput AnalogInput { get; set; }
    public Double Value { get; set; }
    public DateTime Timestamp { get; set; }
    public Guid TagId { get; set; }
    
    public AnalogData()
    {
            
    }

    public AnalogData(Guid id, AnalogInput analogInput, double value, DateTime timestamp, Guid tagId)
    {
        Id = id;
        AnalogInput = analogInput;
        Value = value;
        Timestamp = timestamp;
        TagId = tagId;
    }

}