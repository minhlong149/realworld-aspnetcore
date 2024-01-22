using Core.Entities;

namespace Application.DTOs;

public class AuthorDto
{
    public string? Username { get; init; }
    public string? Bio { get; init; }
    public string? Image { get; init; }

    public class UserProfile : Profile
    {
        public UserProfile() { CreateMap<UserEntity, AuthorDto>(); }
    }
}
