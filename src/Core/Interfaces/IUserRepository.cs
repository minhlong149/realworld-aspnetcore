using Core.Entities;

namespace Core.Interfaces;

public interface IUserRepository: IRepository<UserEntity>
{
    Task<UserEntity?> GetByUsernameAsync(string username);
    Task<UserEntity?> GetByEmailAsync(string email);
}
