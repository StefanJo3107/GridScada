using Backend.Models;

namespace Backend.Repositories;

public interface IAlarmRepository
{
    public interface IAlarmRepository : ICrudRepository<Alarm>
    {
    }
}