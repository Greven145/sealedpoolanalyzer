using System.Reflection;
using Application.Contracts.IInfrastructure;
using Application.Contracts.IPersistence;
using Infrastructure.Brokers.DeckedBuilder;
using Infrastructure.Brokers.Scryfall;
using Infrastructure.Contracts;
using Infrastructure.Data;
using Infrastructure.Data.Parsers;
using Infrastructure.Data.Parsers.Pipeline;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure {
    public static class DependencyInjectionExtensions {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services) {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<ISetLoader, ScryfallBroker>();
            services.AddScoped<IDeckedBuilderLoader,DeckedBuilderBroker>();
            services.AddScoped<DataLoader>();
            services.AddHttpClient<IScryfallFactoryClient, ScryfallFactoryClient>();
            services.AddHttpClient<IDeckedBuilderFactoryClient, DeckedBuilderFactoryClient>();

            services.AddScoped<DeckedBuilderCsvParser>();
            services.AddScoped<DeckedBuilderColl2Parser>();

            services.AddHostedService<DataInitializerService>();

            services.AddSingleton(provider => {
                var scope = provider.CreateScope();
                var serviceProvider = scope.ServiceProvider;
                return new DeckParsingPipelineBuilder<FileParserContext>()
                    .Register(serviceProvider.GetService<DeckedBuilderCsvParser>())
                    .Register(serviceProvider.GetService<DeckedBuilderColl2Parser>())
                    .Build();
            });

            return services;
        }
    }
}
