namespace Backend.Models;

public class AlarmReport
{
    public Alarm Alarm { get; set; }
    public DateTime Timestamp { get; set; }
    public double? Value { get; set; }

    public AlarmReport(Alarm alarm, DateTime timestamp)
    {
        Alarm = alarm;
        Timestamp = timestamp;
    }

    public AlarmReport(Alarm alarm, DateTime timestamp, double value)
    {
        Alarm = alarm;
        Timestamp = timestamp;
        Value = value;
    }

    public AlarmReport()
    {
            
    }
}