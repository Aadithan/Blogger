using System.Reflection;
using System.Runtime.CompilerServices; 
using MyBlog.Application.Common.Interfaces; 
using MyBlog.Infrastructure.Auth;
using MyBlog.Infrastructure.Caching;
using MyBlog.Infrastructure.Common; 
using MyBlog.Infrastructure.Cors;
using MyBlog.Infrastructure.FileStorage; 
using MyBlog.Infrastructure.Identity;  
using MyBlog.Infrastructure.Mailing;
using MyBlog.Infrastructure.Mapping;
using MyBlog.Infrastructure.Middleware;
using MyBlog.Infrastructure.OpenApi;
using MyBlog.Infrastructure.Persistence;
using MyBlog.Infrastructure.Persistence.Context;
using MyBlog.Infrastructure.Persistence.Initialization;
using MyBlog.Infrastructure.Persistence.Interceptors;
using MyBlog.Infrastructure.SecurityHeaders;
using MyBlog.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyBlog.Infrastructure.Localization;

[assembly: InternalsVisibleTo("Infrastructure.Test")]

namespace MyBlog.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AuditableEntitySaveChangesInterceptor>(); 
        MapsterSettings.Configure();
        services
            .AddApiVersioning()
            .AddAuth(configuration)
            .AddCaching(configuration)
            .AddCorsPolicy(configuration)
            .AddPOLocalization(configuration)
            .AddExceptionMiddleware() 
            .AddMailing(configuration)
            .AddMediatR(Assembly.GetExecutingAssembly())
            .AddOpenApiDocumentation(configuration)
            .AddPersistence(configuration)
            .AddRequestLogging(configuration)
            .AddRouting(options => options.LowercaseUrls = true)
            .AddServices()
            .AddCurrentUser();  

        services
            .AddDefaultIdentity<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddTransient<IDateTime, DateTimeService>();
        services.AddTransient<IIdentityService, IdentityService>();  
        //services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<IdentityContext>();

        // add identity
        var builder = services.AddIdentityCore<ApplicationUser>(o =>
        {
            // configure identity options
            o.Password.RequireDigit = false;
            o.Password.RequireLowercase = false;
            o.Password.RequireUppercase = false;
            o.Password.RequireNonAlphanumeric = false;
            o.Password.RequiredLength = 6;
        });
        builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), builder.Services);
        builder.AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
      
        services.AddAuthorization(options =>
            options.AddPolicy("CanPurge", policy => policy.RequireRole("Administrator")));

        return services;
    }

    public static async Task InitializeDatabasesAsync(this IServiceProvider services, CancellationToken cancellationToken = default)
    {
        // Create a new scope to retrieve scoped services
        using var scope = services.CreateScope();
        await scope.ServiceProvider.GetRequiredService<ApplicationDbInitializer>().InitialiseAsync(cancellationToken); 
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder builder, IConfiguration config) =>
        builder
            .UseRequestLocalization()
            .UseStaticFiles()
            .UseSecurityHeaders(config)
            .UseFileStorage()
            .UseExceptionMiddleware() 
            .UseRouting()
            .UseCorsPolicy()
            .UseAuthentication() 
            .UseAuthorization()
            .UseRequestLogging(config) 
            .UseOpenApiDocumentation(config);

    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder builder)
    {
        //builder.MapControllers().RequireAuthorization();
        builder.MapControllers(); 
        return builder;
    }
}
