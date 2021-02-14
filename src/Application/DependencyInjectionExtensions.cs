using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Application {
    public static class DependencyInjectionExtensions {
        public static IServiceCollection AddApplication(this IServiceCollection services) {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}
