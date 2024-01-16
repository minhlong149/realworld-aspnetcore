using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Identity;
using Infrastructure.Repositories;
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

        return services;
    }

    private static IServiceCollection AddPersistence(this IServiceCollection services, string? connectionString)
    {
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddDbContext<ConduitContext>(options => options.UseSqlServer(connectionString));

        using var scope = services.BuildServiceProvider().CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ConduitContext>();
        context.Database.Migrate();

        return services;
    }

    private static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.Key));
        services.AddSingleton<ITokenClaimsService, TokenClaimService>();

        return services;
    }
}
