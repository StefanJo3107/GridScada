namespace Backend.Models;

public class AlarmAlert
{
    public Guid Id { get; set; }
    public Guid AlarmId { get; set; }
    public DateTime Timestamp { get; set; }
    public Double Value { get; set; }
}