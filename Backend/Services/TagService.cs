using Backend.Exceptions;
using Backend.Hubs;
using Backend.Models;
using Backend.Repositories;
using Microsoft.AspNetCore.SignalR;

namespace Backend.Services;

public class TagService(
    IServiceProvider serviceProvider,
    IAnalogInputRepository analogInputRepository,
    IDigitalInputRepository digitalInputRepository,
    IUserRepository userRepository,
    IDigitalDataRepository digitalDataRepository,
    IAnalogDataRepository analogDataRepository,
    IAlarmAlertRepository alarmAlertRepository,
    IHubContext<TagHub, ITagClient> tagHub,
    IHubContext<AlarmHub, IAlarmClient> alarmHub,
    IAlarmRepository alarmRepository)
    : ITagService
{
    public async Task<AnalogInput?> AddAnalogInput(AnalogInput? input, Guid userId)
    {
        if (input == null)
            throw new InvalidInputException("Input cannot be null.");
        if (input != null && input.HighLimit <= input.LowLimit)
            throw new InvalidInputException("High limit cannot be lower than low limit");
        if (input is { ScanTime: <= 0 })
            throw new InvalidInputException("Scan time must be greater than 0");
        var user = await userRepository.Read(userId);
        if (user == null)
            throw new InvalidInputException("User does not exist");

        var guid = Guid.NewGuid();
        input!.Id = guid;
        input.IOAddress = guid.ToString();
        await analogInputRepository.Create(input);
        input.Users.Add(user);
        user.AnalogInputs.Add(input);
        var saved = await userRepository.Update(user);
        await UpdatePermissionsAnalogInputs(
            saved?.AnalogInputs.Last() ?? throw new InvalidOperationException("Analog input cannot be null"),
            userId);
        if (input.ScanOn)
            await StartAnalogTagReading(input.Id);

        return input;
    }


    public async Task<DigitalInput> AddDigitalInput(DigitalInput input, Guid userId)
    {
        if (input.ScanTime <= 0)
            throw new InvalidInputException("Scan time must be greater than 0");
        var user = await userRepository.Read(userId);
        if (user == null)
            throw new InvalidInputException("User does not exist");

        var guid = Guid.NewGuid();
        input.Id = guid;
        input.IOAddress = guid.ToString();
        await digitalInputRepository.Create(input);
        user.DigitalInputs.Add(input);
        var saved = await userRepository.Update(user);
        await UpdatePermissionsDigitalInputs(
            saved?.DigitalInputs.Last() ?? throw new InvalidOperationException("Analog input cannot be null"), userId);
        if (input.ScanOn)
            await StartDigitalTagReading(saved.DigitalInputs.Last().Id);
        return input;
    }

    public async Task SwitchAnalogTag(Guid tagId, Guid userId)
    {
        var user = await userRepository.FindByIdWithTags(userId) ??
                   throw new ResourceNotFoundException("User not found!");
        var analogInput = await analogInputRepository.Read(tagId) ??
                          throw new ResourceNotFoundException("There is no analog tag with this id!");
        if (user.AnalogInputs.All(tag => tag.Id != tagId))
            throw new InvalidInputException("User cannot access other users tags!");
        analogInput.ScanOn = !analogInput.ScanOn;
        await analogInputRepository.Update(analogInput);
        if (analogInput.ScanOn)
            _ = StartAnalogTagReading(tagId);
    }

    public async Task SwitchDigitalTag(Guid tagId, Guid userId)
    {
        var user = await userRepository.FindByIdWithTags(userId) ??
                   throw new ResourceNotFoundException("User not found!");
        var digitalInput = await digitalInputRepository.Read(tagId) ??
                           throw new ResourceNotFoundException("There is no digital tag with this id!");
        if (user.DigitalInputs.All(tag => tag.Id != tagId))
            throw new InvalidInputException("User cannot access other users tags!");
        digitalInput.ScanOn = !digitalInput.ScanOn;
        await digitalInputRepository.Update(digitalInput);
        if (digitalInput.ScanOn)
            _ = StartDigitalTagReading(tagId);
    }

    public async Task DeleteAnalogInput(Guid tagId, Guid userId)
    {
        var user = await userRepository.FindByIdWithTags(userId);
        if (user == null)
            throw new InvalidInputException("User does not exist");

        var tag = await analogInputRepository.FindByIdWithAlarmsAndUsers(tagId);
        if (tag == null)
            throw new InvalidInputException("Tag does not exist");

        if (!user.AnalogInputs.Any(input => input != null && input.Id == tagId))
            throw new ResourceNotFoundException("Tag does net exist");

        user.AnalogInputs.Remove(tag);
        await analogDataRepository.DeleteByTagId(tagId);
        await userRepository.Update(user);
        var users = await userRepository.GetAllByCreatedBy(userId);
        foreach (var u in users)
        {
            u.AnalogInputs.Remove(tag);
            await userRepository.Update(user);
        }

        await analogInputRepository.Delete(tagId);
        foreach (var alarm in tag.Alarms)
        {
            await alarmAlertRepository.DeleteByAlarmId(alarm.Id);
            await alarmRepository.Delete(alarm.Id);
        }
    }

    public async Task DeleteDigitalInput(Guid tagId, Guid userId)
    {
        var user = await userRepository.FindByIdWithTags(userId);
        if (user == null)
            throw new InvalidInputException("User does not exist");

        var tag = await digitalInputRepository.Read(tagId);
        if (tag == null)
            throw new InvalidInputException("Tag does not exist");

        if (user.DigitalInputs.All(input => input.Id != tagId))
            throw new ResourceNotFoundException("Tag does not exist");

        user.DigitalInputs.Remove(tag);
        var users = await userRepository.GetAllByCreatedBy(userId);
        foreach (var u in users)
        {
            u.DigitalInputs.Remove(tag);
            await userRepository.Update(user);
        }

        await userRepository.Update(user);
        await digitalInputRepository.Delete(tag.Id);
    }

    public async Task<TagReport> GetAllTagValuesByTime(Guid userId, DateTime timeFrom, DateTime timeTo)
    {
        var user = await userRepository.FindByIdWithTags(userId) ??
                   throw new ResourceNotFoundException("User not found!");
        var reports = new TagReport([], []);
        foreach (var analogInput in user.AnalogInputs)
        {
            var analogData =
                await analogDataRepository.FindByTagIdByTime(analogInput.Id, timeFrom, timeTo);
            reports.AnalogData.AddRange(analogData);

            var digitalData =
                await digitalDataRepository.FindByTagIdByTime(analogInput.Id, timeFrom, timeTo);
            reports.DigitalData.AddRange(digitalData);
        }

        return reports;
    }

    public async Task<List<AnalogData?>> GetLatestAnalogTagsValues(Guid userId)
    {
        var user = await userRepository.FindByIdWithTags(userId) ??
                   throw new ResourceNotFoundException("User not found!");
        List<AnalogData?> analogDatas = new();
        foreach (var analogInput in user.AnalogInputs)
        {
            var analogData = await analogDataRepository.FindLatestByTagId(analogInput.Id);
            analogDatas.Add(analogData);
        }

        return analogDatas;
    }

    public async Task<List<DigitalData?>> GetLatestDigitalTagsValues(Guid userId)
    {
        var user = await userRepository.FindByIdWithTags(userId) ??
                   throw new ResourceNotFoundException("User not found!");
        List<DigitalData?> digitalDatas = new();
        foreach (var digitalInput in user.DigitalInputs)
        {
            var digitalData = await digitalDataRepository.FindLatestByTagId(digitalInput.Id);
            digitalDatas.Add(digitalData);
        }

        return digitalDatas;
    }

    public async Task<List<AnalogData>> GetAllAnalogTagValues(Guid userId, Guid tagId)
    {
        var user = await userRepository.FindByIdWithTags(userId) ??
                   throw new ResourceNotFoundException("User not found!");
        var analogInput = await analogInputRepository.Read(tagId) ??
                          throw new ResourceNotFoundException("There is no analog tag with this id!");
        if (user.AnalogInputs.All(tag => tag.Id != tagId))
            throw new InvalidInputException("User cannot access other users tags!");

        return await analogDataRepository.FindByTagId(tagId);
    }

    public async Task<List<DigitalData>> GetAllDigitalTagValues(Guid userId, Guid tagId)
    {
        var user = await userRepository.FindByIdWithTags(userId) ??
                   throw new ResourceNotFoundException("User not found!");
        var digitalInput = await digitalInputRepository.Read(tagId) ??
                           throw new ResourceNotFoundException("There is no digital tag with this id!");
        if (user.DigitalInputs.All(tag => tag.Id != tagId))
            throw new InvalidInputException("User cannot access other users tags!");

        return await digitalDataRepository.FindByTagId(tagId);
    }

    public async Task<DigitalInput> GetDigitalInput(Guid tagId, Guid userId)
    {
        var user = await userRepository.FindByIdWithTags(userId);
        if (user == null)
            throw new InvalidInputException("User does not exist");

        var tag = await digitalInputRepository.Read(tagId);
        if (tag == null)
            throw new InvalidInputException("Tag does not exist");

        if (user.AnalogInputs.All(input => input.Id != tagId))
            throw new ResourceNotFoundException("Tag does net exist");

        return tag;
    }

    public async Task<AnalogInput> GetAnalogInput(Guid tagId, Guid userId)
    {
        var user = await userRepository.FindByIdWithTags(userId);
        if (user == null)
            throw new InvalidInputException("User does not exist");

        var tag = await analogInputRepository.Read(tagId);
        if (tag == null)
            throw new InvalidInputException("Tag does not exist");

        if (user.AnalogInputs.All(input => input != null && input.Id != tagId))
            throw new ResourceNotFoundException("Tag does not exist");

        return tag;
    }

    public async Task<AnalogData?> GetLatestAnalogTagValue(Guid tagId, Guid userId)
    {
        var user = await userRepository.FindByIdWithTags(userId) ??
                   throw new ResourceNotFoundException("User not found!");
        var analogData = await analogDataRepository.FindLatestByTagId(tagId);
        return analogData;
    }

    public async Task<DigitalData?> GetLatestDigitalTagValue(Guid tagId, Guid userId)
    {
        var user = await userRepository.FindByIdWithTags(userId) ??
                   throw new ResourceNotFoundException("User not found!");
        var digitalData = await digitalDataRepository.FindLatestByTagId(tagId);
        return digitalData;
    }

    public async Task UpdateAnalog(Guid id, double value, Guid userId)
    {
        var user = await userRepository.FindByIdWithTags(userId) ??
                   throw new ResourceNotFoundException("User not found!");
        var tag = await analogInputRepository.Read(id);
        var analogData = new AnalogData
        {
            Id = new Guid(),
            AnalogInput = tag,
            Value = value,
            Timestamp = DateTime.Now
        };

        await analogDataRepository.Create(analogData);
    }

    public async Task UpdateDigital(Guid id, double value, Guid userId)
    {
        var user = await userRepository.FindByIdWithTags(userId) ??
                   throw new ResourceNotFoundException("User not found!");
        var tag = await digitalInputRepository.Read(id);
        var digitalData = new DigitalData
        {
            Id = new Guid(),
            DigitalInput = tag,
            Value = value,
            Timestamp = DateTime.Now
        };
        await digitalDataRepository.Create(digitalData);
    }

    public async Task StartupCheck()
    {
        foreach (var input in await analogInputRepository.ReadAll())
            if (input.ScanOn)
                _ = StartAnalogTagReading(input.Id);

        foreach (var input in await digitalInputRepository.ReadAll())
            if (input.ScanOn)
                _ = StartDigitalTagReading(input.Id);
    }

    private async Task UpdatePermissionsAnalogInputs(AnalogInput? input, Guid userId)
    {
        var users = await userRepository.GetAllByCreatedBy(userId);
        foreach (var user in users)
        {
            user.AnalogInputs.Add(input);
            await userRepository.Update(user);
        }
    }

    private async Task UpdatePermissionsDigitalInputs(DigitalInput input, Guid userId)
    {
        var users = await userRepository.GetAllByCreatedBy(userId);
        foreach (var user in users)
        {
            user.DigitalInputs.Add(input);
            await userRepository.Update(user);
        }
    }

    private Task StartAnalogTagReading(Guid tagId)
    {
        new Thread(async void () =>
        {
            Thread.CurrentThread.IsBackground = true;
            while (true)
            {
                using var scope = serviceProvider.CreateScope();
                var analogInputRepository = scope.ServiceProvider.GetRequiredService<IAnalogInputRepository>();
                var analogDataRepository = scope.ServiceProvider.GetRequiredService<IAnalogDataRepository>();
                var alarmAlertRepository = scope.ServiceProvider.GetRequiredService<IAlarmAlertRepository>();
                var analogInput = await analogInputRepository.FindByIdWithAlarmsAndUsers(tagId);
                if (analogInput == null)
                    break;
                if (analogInput.ScanOn)
                {
                    var analogData = new AnalogData
                    {
                        Id = new Guid(),
                        AnalogInput = analogInput,
                        Value = analogInput.Value,
                        Timestamp = DateTime.Now
                    };
                    await analogDataRepository.Create(analogData);

                    foreach (var analogInputAlarm in analogInput.Alarms)
                        if ((analogInputAlarm.Type == AlarmType.High &&
                             analogInputAlarm.EdgeValue < analogInput.Value) ||
                            (analogInputAlarm.Type == AlarmType.Low && analogInputAlarm.EdgeValue > analogInput.Value))
                        {
                            var alarmAlert = new AlarmAlert
                            {
                                Id = new Guid(),
                                AlarmId = analogInputAlarm.Id,
                                Timestamp = DateTime.Now,
                                Value = analogInput.Value
                            };
                            await alarmAlertRepository.Create(alarmAlert);
                            await Global.SemaphoreWriter.WaitAsync();

                            try
                            {
                                await using var outputFile = new StreamWriter("alarmLog.txt", true);
                                await outputFile.WriteAsync("Alarm (id: " + alarmAlert.AlarmId +
                                                            ") triggered for tag (id: " + analogInput.Id +
                                                            ") at " + alarmAlert.Timestamp + "\n");
                            }
                            finally
                            {
                                Global.SemaphoreWriter.Release();
                            }

                            var report =
                                new AlarmReport(analogInputAlarm, alarmAlert.Timestamp, analogInput.Value);
                            await alarmHub.Clients.Users(analogInput.Users.Select(u => u.Id.ToString()).ToList())
                                .ReceiveAlarmData(report);
                        }

                    await tagHub.Clients.Users(analogInput.Users.Select(u => u.Id.ToString()).ToList())
                        .ReceiveAnalogData(analogData);
                }
                else
                {
                    break;
                }

                Thread.Sleep(analogInput.ScanTime * 1000);
            }
        }).Start();
        return Task.CompletedTask;
    }

    private Task StartDigitalTagReading(Guid tagId)
    {
        new Thread(async void () =>
        {
            Thread.CurrentThread.IsBackground = true;
            while (true)
            {
                using var scope = serviceProvider.CreateScope();
                var digitalInputRepository = scope.ServiceProvider.GetRequiredService<IDigitalInputRepository>();
                var digitalDataRepository = scope.ServiceProvider.GetRequiredService<IDigitalDataRepository>();

                var digitalInput = await digitalInputRepository.FindByIdWithUsers(tagId);
                if (digitalInput == null)
                    break;

                if (digitalInput.ScanOn)
                {
                    var ioDigitalData = new DigitalData
                    {
                        Id = new Guid(),
                        Value = digitalInput.Value,
                        Timestamp = DateTime.Now,
                        DigitalInput = digitalInput
                    };
                    await digitalDataRepository.Create(ioDigitalData);

                    await tagHub.Clients.Users(digitalInput.Users.Select(u => u.Id.ToString()).ToList())
                        .ReceiveDigitalData(ioDigitalData);
                }
                else
                {
                    break;
                }

                Thread.Sleep(digitalInput.ScanTime * 1000);
            }
        }).Start();
        return Task.CompletedTask;
    }
}