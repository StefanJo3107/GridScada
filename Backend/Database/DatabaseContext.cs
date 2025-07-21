using Backend.Models;
using Microsoft.Extensions.Configuration;

namespace Backend.Database;
using Microsoft.EntityFrameworkCore;

public class DatabaseContext: DbContext   
{
    protected readonly IConfiguration Configuration;

    public DatabaseContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite(Configuration.GetConnectionString("WebApiDatabase"));
    }

    public DbSet<Alarm> Alarm { get; set; }
    public DbSet<AlarmAlert> AlarmAlert { get; set; }
    public DbSet<AnalogInput> AnalogInput { get; set; }
    public DbSet<DigitalInput> DigitalInput { get; set; }
    public DbSet<AnalogData> AnalogData { get; set; }
    public DbSet<DigitalData> DigitalData { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }

}