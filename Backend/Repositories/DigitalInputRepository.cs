using System;
using System.Threading.Tasks;
using Backend.Database;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class DigitalInputRepository(DatabaseContext context) : CrudRepository<DigitalInput>(context), IDigitalInputRepository
{
    public async Task<DigitalInput?> FindByIdWithUsers(Guid id)
    {
        return await Entities.Include(u => u.Users)
            .FirstOrDefaultAsync(e => e.Id == id);
    }
}