using Core.Entities;

namespace Core.Interfaces;

public interface ITokenClaimsService
{
    string GetToken(UserEntity user);
}
