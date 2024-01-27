using Application.DTOs;
using Core.Constants;
using Core.Exceptions;
using Core.Repositories;

namespace Application.Users.Queries.GetProfile;

public class GetProfileHandler(
    IUserRepository userRepository,
    IMapper mapper
) : IRequestHandler<GetProfileRequest, ProfileDto>
{
    public async Task<ProfileDto> Handle(GetProfileRequest request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByUsernameAsync(request.Username);

        if (user is null)
        {
            throw new NotFoundException(UserConstants.UserNotFound);
        }

        return mapper.Map<ProfileDto>(user);
    }
}
