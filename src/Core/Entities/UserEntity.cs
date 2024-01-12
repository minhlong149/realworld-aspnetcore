namespace Core.Entities;

public class UserEntity : BaseEntity
{
    public required string Email { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public string? Bio { get; set; }
    public string? Image { get; set; }
}
