using Backend.Models;

namespace Backend.Services;

public interface ITagService
{
    Task<AnalogInput?> AddAnalogInput(AnalogInput? input, Guid userId);
    Task<DigitalInput> AddDigitalInput(DigitalInput input, Guid userId);
    Task SwitchAnalogTag(Guid tagId, Guid userId);
    Task SwitchDigitalTag(Guid tagId, Guid userId);
    Task DeleteAnalogInput(Guid tagId, Guid userId);
    Task DeleteDigitalInput(Guid tagId, Guid userId);
    Task<TagReport> GetAllTagValuesByTime(Guid userId, DateTime timeFrom, DateTime timeTo);
    Task<List<AnalogData?>> GetLatestAnalogTagsValues(Guid userId);
    Task<List<DigitalData?>> GetLatestDigitalTagsValues(Guid userId);
    Task<List<AnalogData>> GetAllAnalogTagValues(Guid userId, Guid tagId);
    Task<List<DigitalData>> GetAllDigitalTagValues(Guid userId, Guid tagId);
    Task<DigitalInput> GetDigitalInput(Guid tagId, Guid userId);
    Task<AnalogInput> GetAnalogInput(Guid tagId, Guid userId);
    Task<AnalogData?> GetLatestAnalogTagValue(Guid tagId, Guid userId);
    Task<DigitalData?> GetLatestDigitalTagValue(Guid tagId, Guid userId);
    Task UpdateAnalog(Guid id, double value, Guid userId);
    Task UpdateDigital(Guid id, double value, Guid userId);
    Task StartupCheck();
    Task StartSimulation();
}