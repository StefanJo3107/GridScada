namespace Backend.Models;

public class Global
{
    public static SemaphoreSlim Semaphore = new(1);
    public static SemaphoreSlim SemaphoreWriter = new(1);
}