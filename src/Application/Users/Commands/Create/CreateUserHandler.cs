using Application.DTOs;
using Core.Constants;
using Core.Entities;
using Core.Exceptions;
using Core.Repositories;
using Core.Services;

namespace Application.Users.Commands.Create;

public class CreateUserHandler(
    ITokenClaimsService tokenClaimsService,
    IPasswordHasher passwordHasher,
    IUserRepository userRepository,
    IMapper mapper
) : IRequestHandler<CreateUserRequest, UserDto>
{
    public async Task<UserDto> Handle(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var userWithUsername = await userRepository.GetByUsernameAsync(request.Username);

        if (userWithUsername is not null)
        {
            throw new ConflictException(UserConstants.UsernameInUse);
        }

        var userWithEmail = await userRepository.GetByEmailAsync(request.Email);

        if (userWithEmail is not null)
        {
            throw new ConflictException(UserConstants.EmailInUse);
        }

        var newUser = new UserEntity
        {
            Email = request.Email,
            Username = request.Username,
            Password = passwordHasher.HashPassword(request.Password),
            Bio = request.Bio,
            Image = request.Image
        };

        await userRepository.CreateAsync(newUser, cancellationToken);
        
        return mapper.Map<UserDto>(newUser) with { Token = tokenClaimsService.GetToken(newUser) };
    }
}
