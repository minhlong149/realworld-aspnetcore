using Application.DTOs;
using Core.Constants;

namespace Application.Users.Queries.GetProfile;

public class GetProfileValidator : AbstractValidator<ProfileDto>
{
    public GetProfileValidator()
    {
        RuleFor(user => user.Username)
            .NotEmpty().WithMessage(UserConstants.UsernameRequired);
    }
}
