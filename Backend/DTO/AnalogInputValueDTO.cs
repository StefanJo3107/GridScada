using Backend.Models;

namespace Backend.DTO;

public class AnalogInputValueDTO
{
    public AnalogInputValueDTO()
    {
    }

    public AnalogInputValueDTO(AnalogInput input, double value)
    {
        Id = input.Id;
        Description = input.Description;
        IOAddress = input.IOAddress;
        ScanTime = input.ScanTime;
        ScanOn = input.ScanOn;
        LowLimit = input.LowLimit;
        HighLimit = input.HighLimit;
        Value = value;
        Unit = input.Unit;
    }

    public Guid Id { get; set; }
    public string Description { get; set; }
    public string IOAddress { get; set; }
    public int ScanTime { get; set; }
    public bool ScanOn { get; set; }
    public double LowLimit { get; set; }
    public double HighLimit { get; set; }
    public double Value { get; set; }

    public string Unit { get; set; }
}