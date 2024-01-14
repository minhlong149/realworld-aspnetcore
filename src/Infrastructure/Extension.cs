using Core.Interfaces;
using Infrastructure.Data;
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
}
