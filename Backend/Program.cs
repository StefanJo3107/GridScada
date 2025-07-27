using Backend.Database;
using Backend.Hubs;
using Backend.Infrastructure;
using Backend.Repositories;
using Backend.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DatabaseContext>(ServiceLifetime.Transient);

//Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAnalogInputRepository, AnalogInputRepository>();
builder.Services.AddScoped<IDigitalInputRepository, DigitalInputRepository>();
builder.Services.AddScoped<IDigitalDataRepository, DigitalDataRepository>();
builder.Services.AddScoped<IAnalogDataRepository, AnalogDataRepository>();
builder.Services.AddScoped<IAlarmRepository, AlarmRepository>();
builder.Services.AddScoped<IAlarmAlertRepository, AlarmAlertRepository>();

//Services
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton<ITagService, TagService>();
builder.Services.AddSingleton<IAlarmService, AlarmService>();

//Security
builder.Services.AddTransient<CustomCookieAuthenticationEvents>();

builder.Services.AddHostedService<StartupService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.SameSite = SameSiteMode.None;
        options.Cookie.Name = "auth";
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        options.Cookie.MaxAge = options.ExpireTimeSpan;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.EventsType = typeof(CustomCookieAuthenticationEvents);
    });

builder.Services.AddAuthorization();

builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseMiddleware<ExceptionMiddleware>(true);
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();
app.MapHub<TagHub>("/hub/tag");
app.MapHub<AlarmHub>("/hub/alarm");

app.Run();