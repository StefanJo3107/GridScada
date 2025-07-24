using System.Text.Json;
using Backend.Models;

namespace Backend.Services;

public class StartupService : IHostedService
{
    private readonly HttpClient _httpClient;

    public StartupService()
    {
        _httpClient = new HttpClient();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        ParseScadaConfig();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private async void ParseScadaConfig()
    {
        var jsonString = await File.ReadAllTextAsync("scadaConfig.json");
        var jsonDocument = JsonDocument.Parse(jsonString);
        foreach (var property in jsonDocument.RootElement.EnumerateObject())
        {
            var propertyName = property.Name;
            var propertyValue = property.Value;

            switch (propertyName)
            {
                case "frequency":
                    Global.Frequency = int.Parse(propertyValue.ToString());
                    break;
                case "lowLimit":
                    Global.LowLimit = double.Parse(propertyValue.ToString());
                    break;
                case "highLimit":
                    Global.HighLimit = double.Parse(propertyValue.ToString());
                    break;
                case "simulation":
                    Global.Simulation = propertyValue.ToString();
                    break;
            }
        }

        Thread.Sleep(1000);
        _ = await _httpClient.GetAsync("http://localhost:5041/api/Device/startSimulation");
        _ = await _httpClient.GetAsync("http://localhost:5041/api/Tag/startupCheck");
    }
}