using Backend.DTO;

namespace Backend.Models;

public class Alarm : IBaseEntity
{
    public Alarm()
    {
    }

    public Alarm(AlarmDTO alarmDto)
    {
        Id = Guid.NewGuid();
        Type = alarmDto.Type;
        Priority = alarmDto.Priority;
        EdgeValue = alarmDto.EdgeValue;
        Unit = alarmDto.Unit;
    }

    public Alarm(Guid id, AlarmType type, AlarmPriority priority, double edgeValue, string unit,
        AnalogInput analogInput)
    {
        Id = id;
        Type = type;
        Priority = priority;
        EdgeValue = edgeValue;
        Unit = unit;
        AnalogInput = analogInput;
    }

    public AlarmType Type { get; set; }
    public AlarmPriority Priority { get; set; }
    public double EdgeValue { get; set; }
    public string Unit { get; set; }
    public AnalogInput AnalogInput { get; set; }
    public Guid Id { get; set; }
}