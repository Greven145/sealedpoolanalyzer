using System.Reflection;
using Application.Contracts.IInfrastructure;
using Infrastructure.Brokers.Scryfall;
using Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure {
    public static class DependencyInjectionExtensions {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services) {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<ISetLoader, ScryfallBroker>();
            services.AddScoped<DataLoader>();
            services.AddHttpClient<IScryfallFactoryClient, ScryfallFactoryClient>();

            return services;
        }
    }
}
