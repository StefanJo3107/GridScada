using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Models;

namespace Backend.Repositories;

public interface IAnalogDataRepository : ICrudRepository<AnalogData>
{
    Task<List<AnalogData>> FindByTagId(Guid id);
    Task<List<AnalogData>> FindByIdByTime(Guid id, DateTime from, DateTime to);
    Task<AnalogData?> FindLatestById(Guid id);
    Task DeleteByTagId(Guid id);
}