using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SimpleAuth.Services;

namespace SimpleAuth.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSimpleAuth<TContext>(this IServiceCollection services) where TContext : DbContext
        {
            // Register the generic AuthService with the specific DbContext type
            services.AddScoped<IAuthService, AuthService<TContext>>();
            return services;
        }
    }
}
