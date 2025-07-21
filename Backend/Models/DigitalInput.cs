using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Backend.Models;

public class DigitalInput : ITag
{
    public Guid Id { get; set; }
    public required string Description { get; set; }
    public required string IOAddress { get; set; }
    public int ScanTime { get; set; }
    public bool ScanOn { get; set; }
    [JsonIgnore]
    public List<User> Users { get; set; }

    
    public DigitalInput(string description, string iOAddress, int scanTime, bool scanOn)
    {
        Description = description;
        IOAddress = iOAddress;
        ScanTime = scanTime;
        ScanOn = scanOn;
        Users = new List<User>();
    }

    public DigitalInput()
    {
        
    }
    
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