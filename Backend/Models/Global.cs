namespace Backend.Models;

public class Global
{
    public static int Frequency = 2500;
    public static double LowLimit = 0;
    public static double HighLimit = 100;
    public static string Simulation = "SIN";
    public static SemaphoreSlim Semaphore = new(1);
    public static SemaphoreSlim SemaphoreWriter = new(1);
}