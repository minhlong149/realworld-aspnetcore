using Application.DTOs;

namespace Application.Users.Queries.GetProfile;

public record GetProfileRequest(string Username) : IRequest<ProfileDto>;
