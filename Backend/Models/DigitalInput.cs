using System.Text.Json.Serialization;
using Backend.DTO;

namespace Backend.Models;

public class DigitalInput : ITag
{
    public DigitalInput(string description, string iOAddress, int scanTime, bool scanOn, double value)
    {
        Description = description;
        IOAddress = iOAddress;
        ScanTime = scanTime;
        ScanOn = scanOn;
        Value = value;
        Users = new List<User>();
    }

    public DigitalInput(DigitalInputDTO digitalInputDTO)
    {
        Description = digitalInputDTO.Description;
        ScanTime = digitalInputDTO.ScanTime;
        ScanOn = digitalInputDTO.ScanOn;
        Users = new List<User>();
    }


    public DigitalInput()
    {
    }

    public string Description { get; set; }
    public int ScanTime { get; set; }
    public bool ScanOn { get; set; }

    [JsonIgnore] public List<User> Users { get; set; }

    public Guid Id { get; set; }
    public string IOAddress { get; set; }
    public double Value { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is DigitalInput input &&
               Id.Equals(input.Id) &&
               Description == input.Description &&
               IOAddress == input.IOAddress &&
               ScanTime == input.ScanTime &&
               ScanOn == input.ScanOn;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Description, IOAddress, ScanTime, ScanOn);
    }
}