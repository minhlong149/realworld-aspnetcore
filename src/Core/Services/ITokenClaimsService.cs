using Core.Entities;

namespace Core.Services;

public interface ITokenClaimsService
{
    string GetToken(UserEntity user);
}
