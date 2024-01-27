using Application.DTOs;
using Core.Constants;
using Core.Exceptions;
using Core.Repositories;
using Core.Services;

namespace Application.Features.Users;

public record Authentication : IRequest<UserDto>
{
    public required string Email { get; init; }

    public required string Password { get; init; }
}

public class AuthenticationValidator : AbstractValidator<Authentication>
{
    public AuthenticationValidator()
    {
        RuleFor(user => user.Email)
            .NotEmpty().WithMessage(UserConstants.EmailRequired);

        RuleFor(user => user.Email)
            .EmailAddress().WithMessage(UserConstants.EmailInvalid);

        RuleFor(user => user.Password)
            .NotEmpty().WithMessage(UserConstants.PasswordRequired);
    }
}

public class AuthenticationHandler(
    ITokenClaimsService tokenClaimsService,
    IPasswordHasher passwordHasher,
    IUserRepository userRepository,
    IMapper mapper
) : IRequestHandler<Authentication, UserDto>
{
    public async Task<UserDto> Handle(Authentication request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(request.Email);

        if (user is null || !passwordHasher.VerifyHashedPassword(request.Password, user.Password))
        {
            throw new InvalidCredentialsException(UserConstants.InvalidCredentials);
        }

        return mapper.Map<UserDto>(user) with { Token = tokenClaimsService.GetToken(user) };
    }
}
