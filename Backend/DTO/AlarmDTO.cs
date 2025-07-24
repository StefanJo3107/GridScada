using Backend.Models;

namespace Backend.DTO;

public class AlarmDTO
{
    public AlarmType Type { get; set; }
    public AlarmPriority Priority { get; set; }
    public double EdgeValue { get; set; }
    public string Unit { get; set; }
    public Guid TagId { get; set; }
}