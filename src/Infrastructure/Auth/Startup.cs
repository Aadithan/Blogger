using MyBlog.Application.Common.Interfaces;
using MyBlog.Infrastructure.Auth.jwt;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyBlog.Infrastructure.Identity;

namespace MyBlog.Infrastructure.Auth;

internal static class Startup
{
    internal static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration config)
    {
        services
            .AddCurrentUser()
            .AddIdentity();
        // Must add identity before adding auth!
        //Todo

        services.Configure<SecuritySettings>(config.GetSection(nameof(SecuritySettings)));
        if (config["SecuritySettings:Provider"].Equals("Jwt", StringComparison.OrdinalIgnoreCase))
        {
            services.AddJwtAuth(config);
        }
        else
        {
            //Todo
        }

        return services;
    }

    internal static IApplicationBuilder UseCurrentUser(this IApplicationBuilder app) =>
        app.UseMiddleware<CurrentUserMiddleware>();

    private static IServiceCollection AddCurrentUser(this IServiceCollection services) =>
        services
            .AddScoped<CurrentUserMiddleware>()
            .AddScoped<ICurrentUser, CurrentUser>()
            .AddScoped(sp => (ICurrentUserInitializer)sp.GetRequiredService<ICurrentUser>());
}