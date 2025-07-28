using Backend.DTO;
using Backend.Exceptions;
using Backend.Models;
using Backend.Repositories;

namespace Backend.Services;

public class AlarmService(
    IAlarmRepository alarmRepository,
    IAnalogInputRepository analogInputRepository,
    IUserRepository userRepository,
    IAlarmAlertRepository alarmAlertRepository)
    : IAlarmService
{
    public async Task MakeAlarm(Guid userId, AlarmDTO alarm)
    {
        var user = await userRepository.FindByIdWithTags(userId) ??
                   throw new ResourceNotFoundException("User not found!");
        var analogInput = await analogInputRepository.FindByIdWithAlarmsAndUsers(alarm.TagId) ??
                          throw new ResourceNotFoundException("There is no analog tag with this id!");
        if (user.AnalogInputs.All(tag =>
                !tag.Id.ToString().ToLower().Equals(alarm.TagId.ToString().ToLower())))
            throw new InvalidInputException("User cannot access other users tags!");

        var newAlarm = new Alarm(Guid.NewGuid(), alarm.Type, alarm.Priority, alarm.EdgeValue, alarm.Unit,
            analogInput);
        analogInput.Alarms.Add(newAlarm);
        await alarmRepository.Create(newAlarm);
        await analogInputRepository.Update(analogInput);
    }

    public async Task DeleteAlarm(Guid userId, Guid alarmId, Guid tagId)
    {
        var user = await userRepository.FindByIdWithTags(userId) ??
                   throw new ResourceNotFoundException("User not found!");
        var analogInput = await analogInputRepository.FindByIdWithAlarmsAndUsers(tagId) ??
                          throw new ResourceNotFoundException("There is no analog tag with this id!");
        if (user.AnalogInputs.All(tag =>
                tag != null && !tag.Id.ToString().ToLower().Equals(tagId.ToString().ToLower())))
            throw new InvalidInputException("User cannot access other users tags!");

        await alarmAlertRepository.DeleteByAlarmId(alarmId);
        analogInput.Alarms = analogInput.Alarms
            .Where(alarm => !alarm.Id.ToString().ToLower().Equals(alarmId.ToString().ToLower())).ToList();
        await analogInputRepository.Update(analogInput);
        await alarmRepository.Delete(alarmId);
    }

    public async Task<List<AlarmReport>> GetAllAlarmsByTime(Guid userId, DateTime timeFrom, DateTime timeTo)
    {
        var user = await userRepository.FindByIdWithTags(userId) ??
                   throw new ResourceNotFoundException("User not found!");
        List<AlarmReport> alarmReports = new();
        foreach (var analogInput in user.AnalogInputs)
        foreach (var analogInputAlarm in analogInput?.Alarms)
        {
            var alarmAlerts =
                (List<AlarmAlert>)await alarmAlertRepository.FindByIdByTime(analogInputAlarm.Id, timeFrom, timeTo);
            var alarmReportsDtos = alarmAlerts.Select(item => new AlarmReport
            {
                Alarm = analogInputAlarm,
                Timestamp = item.Timestamp,
                Value = item.Value
            }).ToList();
            alarmReports.AddRange(alarmReportsDtos);
        }

        return alarmReports.OrderBy(item => item.Timestamp).ToList();
    }

    public async Task<List<AlarmReport>> GetAllAlarmsByPriority(Guid userId, AlarmPriority priority)
    {
        var user = await userRepository.FindByIdWithTags(userId) ??
                   throw new ResourceNotFoundException("User not found!");
        List<AlarmReport> alarmReports = new();
        foreach (var analogInput in user.AnalogInputs)
        foreach (var analogInputAlarm in analogInput.Alarms)
        {
            if (analogInputAlarm.Priority != priority) continue;
            var alarmAlerts =
                await alarmAlertRepository.FindByAlarmId(analogInputAlarm.Id);
            var alarmReportsDtos = alarmAlerts.Select(item => new AlarmReport
            {
                Alarm = analogInputAlarm,
                Timestamp = item.Timestamp,
                Value = item.Value
            }).ToList();
            alarmReports.AddRange(alarmReportsDtos);
        }

        return alarmReports.OrderBy(item => item.Timestamp).ToList();
    }
}