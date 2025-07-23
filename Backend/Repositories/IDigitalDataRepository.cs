using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Models;

namespace Backend.Repositories;

public interface IDigitalDataRepository : ICrudRepository<DigitalData>
{
    Task<List<DigitalData>> FindByTagId(Guid id);
    Task<List<DigitalData>> FindByTagIdByTime(Guid id, DateTime from, DateTime to);
    Task<DigitalData?> FindLatestByTagId(Guid id);
    Task DeleteByTagId(Guid id);
}