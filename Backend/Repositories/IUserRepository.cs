using Backend.Models;

namespace Backend.Repositories;

public interface IUserRepository: ICrudRepository<User>
{
    public Task<User?> FindByEmail(String email);
    public Task<User?> FindByIdWithTags(Guid id);
    public Task<List<User>> GetAllByCreatedBy(Guid userId);
}