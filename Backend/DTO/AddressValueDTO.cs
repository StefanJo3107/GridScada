namespace Backend.DTO;

public class AddressValueDTO
{
    public AddressValueDTO(Guid id, double value)
    {
        Id = id;
        Value = value;
    }

    public AddressValueDTO()
    {
    }

    public Guid Id { get; set; }
    public double Value { get; set; }
}