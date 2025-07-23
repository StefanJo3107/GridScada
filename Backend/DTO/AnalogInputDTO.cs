namespace Backend.DTO;

public class AnalogInputDTO
{
    public AnalogInputDTO(string description, int scanTime, bool scanOn, double lowLimit,
        double highLimit, string unit)
    {
        Description = description;
        ScanTime = scanTime;
        ScanOn = scanOn;
        LowLimit = lowLimit;
        HighLimit = highLimit;
        Unit = unit;
    }

    public string Description { get; set; }
    public int ScanTime { get; set; }
    public bool ScanOn { get; set; }
    public double LowLimit { get; set; }
    public double HighLimit { get; set; }
    public string Unit { get; set; }
}