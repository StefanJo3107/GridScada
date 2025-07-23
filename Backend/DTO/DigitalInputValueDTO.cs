using Backend.Models;

namespace Backend.DTO;

public class DigitalInputValueDTO
{
    public DigitalInputValueDTO()
    {
    }

    public DigitalInputValueDTO(DigitalInput input, double value)
    {
        Id = input.Id;
        Description = input.Description;
        IOAddress = input.IOAddress;
        ScanTime = input.ScanTime;
        ScanOn = input.ScanOn;
        Value = value;
    }

    public Guid Id { get; set; }
    public string Description { get; set; }
    public string IOAddress { get; set; }
    public int ScanTime { get; set; }
    public bool ScanOn { get; set; }
    public double Value { get; set; }
}