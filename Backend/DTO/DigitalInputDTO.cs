namespace Backend.DTO;

public class DigitalInputDTO
{
    public DigitalInputDTO(string description, string driver, int scanTime, bool scanOn)
    {
        Description = description;
        ScanTime = scanTime;
        ScanOn = scanOn;
    }

    public string Description { get; set; }
    public int ScanTime { get; set; }
    public bool ScanOn { get; set; }
}