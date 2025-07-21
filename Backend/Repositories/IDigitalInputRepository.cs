using System;
using System.Threading.Tasks;
using Backend.Models;

namespace Backend.Repositories;

public interface IDigitalInputRepository : ICrudRepository<DigitalInput>
{
    public Task<DigitalInput?> FindByIdWithUsers(Guid id);
}
