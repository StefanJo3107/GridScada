namespace Backend.DTO;

public class TagsDTO
{
    public TagsDTO(List<AnalogInputValueDTO> analogInputs, List<DigitalInputValueDTO> digitalInputs)
    {
        AnalogInputs = analogInputs;
        DigitalInputs = digitalInputs;
    }

    public List<AnalogInputValueDTO> AnalogInputs { get; set; }
    public List<DigitalInputValueDTO> DigitalInputs { get; set; }
}