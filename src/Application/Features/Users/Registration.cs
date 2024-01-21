using Application.DTOs;
using Core.Constants;
using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;

namespace Application.Features.Users;

public record Registration : IRequest<UserDto>
{
    public required string Email { get; init; }

    public required string Username { get; init; }

    public required string Password { get; init; }
}

public class RegistrationValidator : AbstractValidator<Registration>
{
    public RegistrationValidator()
    {
        RuleFor(user => user.Email)
            .NotEmpty().WithMessage(UserConstants.EmailRequired);

        RuleFor(user => user.Username)
            .NotEmpty().WithMessage(UserConstants.UsernameRequired);

        RuleFor(user => user.Email)
            .EmailAddress().WithMessage(UserConstants.EmailInvalid);

        RuleFor(user => user.Password)
            .NotEmpty().WithMessage(UserConstants.PasswordRequired);
    }
}

public class RegistrationHandler(
    ITokenClaimsService tokenClaimsService,
    IPasswordHasher passwordHasher,
    IUserRepository userRepository,
    IMapper mapper
) : IRequestHandler<Registration, UserDto>
{
    public async Task<UserDto> Handle(Registration request, CancellationToken cancellationToken)
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
            Email = request.Email, Username = request.Username, 
            Password = passwordHasher.HashPassword(request.Password)
        };

        await userRepository.CreateAsync(newUser, cancellationToken);

        return mapper.Map<UserDto>(newUser) with { Token = tokenClaimsService.GetToken(newUser) };
    }
}
