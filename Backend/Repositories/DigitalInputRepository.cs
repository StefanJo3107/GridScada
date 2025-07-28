using Backend.Database;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class DigitalInputRepository(DatabaseContext context)
    : CrudRepository<DigitalInput>(context), IDigitalInputRepository
{
    public async Task<DigitalInput?> FindByIdWithUsers(Guid id)
    {
        await Global.Semaphore.WaitAsync();

        try
        {
            await Task.Delay(1);
            return await Entities.Include(u => u.Users)
                .FirstOrDefaultAsync(e => e.Id.ToString().ToLower().Equals(id.ToString().ToLower()));
        }
        finally
        {
            Global.Semaphore.Release();
        }
    }
}