using System;
using System.Threading.Tasks;
using Backend.Models;

namespace Backend.Repositories;

public interface IAnalogInputRepository:ICrudRepository<AnalogInput>
{
    public Task<AnalogInput?> FindByIdWithAlarmsAndUsers(Guid id);
    Task DeleteAlarms(Guid tagId);
}
