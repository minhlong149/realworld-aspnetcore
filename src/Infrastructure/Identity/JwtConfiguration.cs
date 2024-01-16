using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Identity;

public class JwtConfiguration(IOptions<JwtSettings> jwtSettings) : IConfigureNamedOptions<JwtBearerOptions>
{
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;

    public void Configure(JwtBearerOptions options)
    {
        var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    }

    public void Configure(string? name, JwtBearerOptions options) => Configure(options);
}
