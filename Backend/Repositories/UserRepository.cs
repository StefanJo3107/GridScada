using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Database;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class UserRepository(DatabaseContext context) : CrudRepository<User>(context), IUserRepository
{
    public async Task<User?> FindByEmail(string email)
    {
        return await Entities.FirstOrDefaultAsync(e => e.Email == email);
        
    }

    public async Task<User?> FindByIdWithTags(Guid id)
    {
        return await Entities
            .Include(e => e.AnalogInputs).ThenInclude(e => e.Alarms)
            .Include(e => e.DigitalInputs)
            .FirstOrDefaultAsync(e => e.Id == id);    }

    public async Task<List<User>> GetAllByCreatedBy(Guid userId)
    {
        return await Entities.Where(e => e.CreatedBy != null && e.CreatedBy.Id == userId).ToListAsync();
    }
}