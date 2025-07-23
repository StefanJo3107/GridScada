using System.Text.Json.Serialization;
using Backend.DTO;

namespace Backend.Models;

public class AnalogInput : ITag
{
    public AnalogInput(string description, string iOAddress, int scanTime, List<Alarm> alarms,
        bool scanOn, double lowLimit, double highLimit, string unit, double value)
    {
        Description = description;
        IOAddress = iOAddress;
        ScanTime = scanTime;
        Alarms = alarms;
        ScanOn = scanOn;
        LowLimit = lowLimit;
        HighLimit = highLimit;
        Unit = unit;
        Value = value;
        Users = new List<User>();
    }

    public AnalogInput(AnalogInputDTO analogInputDTO)
    {
        Description = analogInputDTO.Description;
        ScanTime = analogInputDTO.ScanTime;
        Alarms = new List<Alarm>();
        ScanOn = analogInputDTO.ScanOn;
        LowLimit = analogInputDTO.LowLimit;
        HighLimit = analogInputDTO.HighLimit;
        Unit = analogInputDTO.Unit;
        Users = new List<User>();
    }

    public AnalogInput()
    {
    }

    public string Description { get; set; }
    public int ScanTime { get; set; }
    public bool ScanOn { get; set; }
    public double LowLimit { get; set; }
    public double HighLimit { get; set; }
    public string Unit { get; set; }

    [JsonIgnore] public List<Alarm> Alarms { get; set; }

    [JsonIgnore] public List<User> Users { get; set; }

    public Guid Id { get; set; }
    public string IOAddress { get; set; }
    public double Value { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is AnalogInput input &&
               Id.Equals(input.Id) &&
               Description == input.Description &&
               IOAddress == input.IOAddress &&
               ScanTime == input.ScanTime &&
               EqualityComparer<List<Alarm>>.Default.Equals(Alarms, input.Alarms) &&
               ScanOn == input.ScanOn &&
               LowLimit == input.LowLimit &&
               HighLimit == input.HighLimit &&
               Unit == input.Unit &&
               EqualityComparer<List<User>>.Default.Equals(Users, input.Users);
    }

    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(Id);
        hash.Add(Description);
        hash.Add(IOAddress);
        hash.Add(ScanTime);
        hash.Add(Alarms);
        hash.Add(ScanOn);
        hash.Add(LowLimit);
        hash.Add(HighLimit);
        hash.Add(Unit);
        hash.Add(Users);
        return hash.ToHashCode();
    }
}