using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Database;

public class DatabaseContext : DbContext
{
    protected readonly IConfiguration Configuration;

    public DatabaseContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public DbSet<Alarm> Alarm { get; set; }
    public DbSet<AlarmAlert> AlarmAlert { get; set; }
    public DbSet<AnalogInput> AnalogInput { get; set; }
    public DbSet<DigitalInput> DigitalInput { get; set; }
    public DbSet<AnalogData> AnalogData { get; set; }
    public DbSet<DigitalData> DigitalData { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite(Configuration.GetConnectionString("WebApiDatabase"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(new User(Guid.Parse("898b1f78-ddfa-4f84-8349-3f8ec62f52bf"), "Admin",
            "Admin", "admin@example.com",
            "$2a$12$G7/QkZt1iK9viHX6HYa73OtnmgF/xYeJOf8klJ9XjnoYBEkHAWZhu", "Admin", null));

        modelBuilder.Entity<User>()
            .HasIndex(c => new { c.Email })
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasMany(e => e.AnalogInputs)
            .WithMany(e => e.Users)
            .UsingEntity("UsersToAnalogInputsJoinTable");

        modelBuilder.Entity<User>()
            .HasMany(e => e.DigitalInputs)
            .WithMany(e => e.Users)
            .UsingEntity("UsersToDigitalInputsJoinTable");
    }
}