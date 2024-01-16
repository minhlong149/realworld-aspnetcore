namespace Infrastructure.Identity;

public class JwtSettings
{
    public const string Key = "Authentication";
    public required string SecretKey { get; init; }
    public required int ExpiryInHours { get; init; }
}
