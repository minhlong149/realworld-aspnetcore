using Application.DTOs;
using Core.Constants;
using Core.Exceptions;
using Core.Repositories;
using Core.Services;

namespace Application.Features.Users;

public record UpdateUser(Guid Id) : IRequest<UserDto>
{
    public string? Email { get; init; }
    public string? Username { get; init; }
    public string? Password { get; init; }
    public string? Bio { get; init; }
    public string? Image { get; init; }
}

public class UpdateUserValidator : AbstractValidator<UpdateUser>
{
    public UpdateUserValidator()
    {
        RuleFor(user => user.Email)
            .NotEmpty().WithMessage(UserConstants.EmailRequired)
            .EmailAddress().WithMessage(UserConstants.EmailInvalid)
            .When(user => user.Email is not null);

        RuleFor(user => user.Username)
            .NotEmpty().WithMessage(UserConstants.UsernameRequired)
            .When(user => user.Username is not null);

        RuleFor(user => user.Password)
            .NotEmpty().WithMessage(UserConstants.PasswordRequired)
            .When(user => user.Password is not null);

        RuleFor(user => user.Bio)
            .NotEmpty().WithMessage(UserConstants.BioRequired)
            .When(user => user.Bio is not null);

        RuleFor(user => user.Image)
            .NotEmpty().WithMessage(UserConstants.ImageRequired)
            .When(user => user.Image is not null);
    }
}

public class UpdateUserHandler(
    ITokenClaimsService tokenClaimsService,
    IPasswordHasher passwordHasher,
    IUserRepository userRepository,
    IMapper mapper
) : IRequestHandler<UpdateUser, UserDto>
{
    public async Task<UserDto> Handle(UpdateUser request, CancellationToken cancellationToken)
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
