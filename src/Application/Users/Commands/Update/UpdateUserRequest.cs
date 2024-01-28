using Application.DTOs;

namespace Application.Users.Commands.Update;

public record UpdateUserRequest(Guid Id) : IRequest<UserDto>
{
    public string? Email { get; init; }
    public string? Username { get; init; }
    public string? Password { get; init; }
    public string? Bio { get; init; }
    public string? Image { get; init; }
}
