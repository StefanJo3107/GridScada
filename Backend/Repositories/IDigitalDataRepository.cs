using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Models;

namespace Backend.Repositories;

public interface IDigitalDataRepository : ICrudRepository<DigitalData>
{
    Task<List<DigitalData>> FindByTagId(Guid id);
    Task<List<DigitalData>> FindByIdByTime(Guid id, DateTime from, DateTime to);
    Task<DigitalData?> FindLatestById(Guid id);
    Task DeleteByTag(Guid id);
}