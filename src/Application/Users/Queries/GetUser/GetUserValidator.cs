using Core.Constants;

namespace Application.Users.Queries.GetUser;

public class GetUserValidator : AbstractValidator<GetUserRequest>
{
    public GetUserValidator()
    {
        RuleFor(user => user.Email)
            .NotEmpty().WithMessage(UserConstants.EmailRequired);

        RuleFor(user => user.Email)
            .EmailAddress().WithMessage(UserConstants.EmailInvalid);

        RuleFor(user => user.Password)
            .NotEmpty().WithMessage(UserConstants.PasswordRequired);
    }
}
