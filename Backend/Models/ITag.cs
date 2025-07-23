namespace Backend.Models;

public interface ITag : IBaseEntity
{
    public string IOAddress { get; set; }
    public double Value { get; set; }
}