using Application.DTOs;
using Core.Constants;
using Core.Exceptions;
using Core.Repositories;
using Core.Services;

namespace Application.Users.Commands.Update;

public class UpdateUserHandler(
    ITokenClaimsService tokenClaimsService,
    IPasswordHasher passwordHasher,
    IUserRepository userRepository,
    IMapper mapper
) : IRequestHandler<UpdateUserRequest, UserDto>
{
    public async Task<UserDto> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var updateUser = await userRepository.GetByIdAsync(request.Id);

        if (updateUser is null)
        {
            throw new NotFoundException(UserConstants.UserNotFound);
        }

        if (request.Email is not null)
        {
            var userWithEmail = await userRepository.GetByEmailAsync(request.Email);

            if (userWithEmail is not null && userWithEmail.Id != request.Id)
            {
                throw new ConflictException(UserConstants.EmailInUse);
            }

            updateUser.Email = request.Email;
        }

        if (request.Username is not null)
        {
            var userWithUsername = await userRepository.GetByUsernameAsync(request.Username);

            if (userWithUsername is not null && userWithUsername.Id != request.Id)
            {
                throw new ConflictException(UserConstants.UsernameInUse);
            }

            updateUser.Username = request.Username;
        }

        if (request.Password is not null)
        {
            updateUser.Password = passwordHasher.HashPassword(request.Password);
        }

        if (request.Bio is not null)
        {
            updateUser.Bio = request.Bio;
        }

        if (request.Image is not null)
        {
            updateUser.Image = request.Image;
        }

        await userRepository.UpdateAsync(updateUser, cancellationToken);
        
        return mapper.Map<UserDto>(updateUser) with { Token = tokenClaimsService.GetToken(updateUser) };
    }
}
