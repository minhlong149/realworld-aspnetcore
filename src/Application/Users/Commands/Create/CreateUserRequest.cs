using Application.DTOs;

namespace Application.Users.Commands.Create;

public record CreateUserRequest : IRequest<UserDto>
{
    public required string Email { get; init; }
    public required string Username { get; init; }
    public required string Password { get; init; }
    public string? Bio { get; init; }
    public string? Image { get; init; }
}
