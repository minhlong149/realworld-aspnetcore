using Application.DTOs;
using Core.Constants;
using Core.Exceptions;
using Core.Repositories;

namespace Application.Features.Users;

public record GetProfile(string Username) : IRequest<ProfileDto>;

public class GetProfileValidator : AbstractValidator<ProfileDto>
{
    public GetProfileValidator()
    {
        RuleFor(user => user.Username)
            .NotEmpty().WithMessage(UserConstants.UsernameRequired);
    }
}

public class GetProfileHandler(IUserRepository userRepository, IMapper mapper) : IRequestHandler<GetProfile, ProfileDto>
{
    public async Task<ProfileDto> Handle(GetProfile request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByUsernameAsync(request.Username);

        if (user is null)
        {
            throw new NotFoundException(UserConstants.UserNotFound);
        }

        return mapper.Map<ProfileDto>(user);
    }
}
