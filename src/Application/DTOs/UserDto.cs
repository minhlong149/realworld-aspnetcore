using Core.Entities;

namespace Application.DTOs;

public record UserDto
{
    public string? Email { get; init; }
    public string? Username { get; init; }
    public string? Bio { get; init; }
    public string? Image { get; init; }
    public string? Token { get; init; }

    public class UserProfile : Profile
    {
        public UserProfile() { CreateMap<UserEntity, UserDto>(); }
    }
}
