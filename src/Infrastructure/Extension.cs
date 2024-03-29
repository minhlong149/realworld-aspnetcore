using Core.Repositories;
using Core.Services;
using Infrastructure.Data;
using Infrastructure.Identity;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class Extension
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddPersistence(configuration.GetConnectionString("DefaultConnection"));
        services.AddAuthentication(configuration);
        services.AddServices();

        return services;
    }

    private static IServiceCollection AddPersistence(this IServiceCollection services, string? connectionString)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IArticleRepository, ArticleRepository>();

        services.AddDbContext<ConduitContext>(options => options.UseSqlServer(connectionString));

        using var scope = services.BuildServiceProvider().CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ConduitContext>();
        context.Database.Migrate();

        return services;
    }

    private static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.Key));
        services.ConfigureOptions<JwtConfiguration>()
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();
        
        return services;
    }
    
    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<ITokenClaimsService, TokenClaimService>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<ISlugGenerator, SlugGenerator>();
        
        return services;
    }
}
