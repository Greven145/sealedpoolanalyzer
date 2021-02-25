using System.Reflection;
using Application.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application {
    public static class DependencyInjectionExtensions {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration) {
            services.Configure<SetsToLoad>(configuration.GetSection("SetsToLoad"));
            services.AddSingleton(new DeckedBuilderVersion());
            
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}
