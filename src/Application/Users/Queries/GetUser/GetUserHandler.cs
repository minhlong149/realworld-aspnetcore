using Application.DTOs;
using Core.Constants;
using Core.Exceptions;
using Core.Repositories;
using Core.Services;

namespace Application.Users.Queries.GetUser;

public class GetUserHandler(
    ITokenClaimsService tokenClaimsService,
    IPasswordHasher passwordHasher,
    IUserRepository userRepository,
    IMapper mapper
) : IRequestHandler<GetUserRequest, UserDto>
{
    public async Task<UserDto> Handle(GetUserRequest request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(request.Email);

        if (user is null || !passwordHasher.VerifyHashedPassword(request.Password, user.Password))
        {
            throw new InvalidCredentialsException(UserConstants.InvalidCredentials);
        }
        
        return mapper.Map<UserDto>(user) with { Token = tokenClaimsService.GetToken(user) };
    }
}
