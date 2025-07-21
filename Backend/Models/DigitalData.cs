using System;

namespace Backend.Models;

public class DigitalData : IBaseEntity
{
    public Guid Id { get; set; }
    public DigitalInput DigitalInput { get; set; }
    public Double Value { get; set; }
    public DateTime Timestamp { get; set; }
    public Guid TagId { get; set; }
    
    public DigitalData()
    {
            
    }

    public DigitalData(Guid id, DigitalInput digitalInput, Double value, DateTime timestamp, Guid tagId)
    {
        Id = id;
        DigitalInput = digitalInput;
        Value = value;
        Timestamp = timestamp;
        TagId = tagId;
    }

}