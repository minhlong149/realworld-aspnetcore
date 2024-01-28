using Core.Constants;

namespace Application.Users.Commands.Create;

public class CreateUserValidator: AbstractValidator<CreateUserRequest>
{
    public CreateUserValidator()
    {
        RuleFor(user => user.Email)
            .EmailAddress().WithMessage(UserConstants.EmailInvalid)
            .NotEmpty().WithMessage(UserConstants.EmailRequired);

        RuleFor(user => user.Username)
            .NotEmpty().WithMessage(UserConstants.UsernameRequired);
        
        RuleFor(user => user.Password)
            .NotEmpty().WithMessage(UserConstants.PasswordRequired);
    }
}
