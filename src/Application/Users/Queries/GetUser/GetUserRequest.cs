using Application.DTOs;

namespace Application.Users.Queries.GetUser;

public record GetUserRequest : IRequest<UserDto>
{
    public required string Email { get; init; }

    public required string Password { get; init; }
};
