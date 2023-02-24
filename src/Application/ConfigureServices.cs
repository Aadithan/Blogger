using Microsoft.Extensions.DependencyInjection;

namespace MyBlog.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        return services
            .AddAutoMapper(assembly)
            .AddValidatorsFromAssembly(assembly)
            .AddMediatR(assembly); 
    }
}
