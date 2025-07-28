using Backend.Database;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class UserRepository(DatabaseContext context) : CrudRepository<User>(context), IUserRepository
{
    public async Task<User?> FindByEmail(string email)
    {
        await Global.Semaphore.WaitAsync();

        try
        {
            await Task.Delay(1);
            return await Entities.FirstOrDefaultAsync(e => e.Email.Equals(email));
        }
        finally
        {
            Global.Semaphore.Release();
        }
    }

    public async Task<User?> FindByIdWithTags(Guid id)
    {
        await Global.Semaphore.WaitAsync();

        try
        {
            await Task.Delay(1);
            var user = await Entities
                .Include(e => e.AnalogInputs).ThenInclude(e => e.Alarms)
                .Include(e => e.DigitalInputs)
                .FirstOrDefaultAsync(e => e.Id.ToString().ToLower().Equals(id.ToString().ToLower()));
            return user;
        }
        finally
        {
            Global.Semaphore.Release();
        }
    }

    public async Task<List<User>> GetAllByCreatedBy(Guid userId)
    {
        await Global.Semaphore.WaitAsync();

        try
        {
            await Task.Delay(1);

            return await Entities.Where(e =>
                    e.CreatedBy != null && e.CreatedBy.Id.ToString().ToLower().Equals(userId.ToString().ToLower()))
                .ToListAsync();
        }
        finally
        {
            Global.Semaphore.Release();
        }
    }
}