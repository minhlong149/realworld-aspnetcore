using Core.Entities;

namespace Application.DTOs;

public record ProfileDto
{
    public string? Email { get; init; }
    public string? Username { get; init; }
    public string? Bio { get; init; }
    public string? Image { get; init; }

    public class UserProfile : Profile
    {
        public UserProfile() { CreateMap<UserEntity, ProfileDto>(); }
    }
}
