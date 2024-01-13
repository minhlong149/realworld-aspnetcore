using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class UserRepository(ConduitContext context) : Repository<UserEntity>(context), IUserRepository
{
    public Task<UserEntity?> GetByUsernameAsync(string username)
    {
        return _dbSet.SingleOrDefaultAsync(user => user.Username == username);
    }
}
