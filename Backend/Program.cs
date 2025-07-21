using Backend.Database;
using Backend.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<DatabaseContext>();

//Repositories
builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddSingleton<IAnalogInputRepository, AnalogInputRepository>();
builder.Services.AddSingleton<IDigitalInputRepository, DigitalInputRepository>();
builder.Services.AddSingleton<IDigitalDataRepository, DigitalDataRepository>();
builder.Services.AddSingleton<IAnalogDataRepository, AnalogDataRepository>();
builder.Services.AddSingleton<IAlarmRepository, AlarmRepository>();
builder.Services.AddSingleton<IAlarmAlertRepository, AlarmAlertRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();