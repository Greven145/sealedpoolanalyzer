using System.Reflection;
using Application.Contracts.IInfrastructure;
using Infrastructure.Brokers.Scryfall;
using Infrastructure.Data;
using Infrastructure.Data.Parsers;
using Infrastructure.Data.Parsers.Pipeline;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure {
    public static class DependencyInjectionExtensions {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services) {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<ISetLoader, ScryfallBroker>();
            services.AddScoped<DataLoader>();
            services.AddHttpClient<IScryfallFactoryClient, ScryfallFactoryClient>();
            
            services.AddHostedService<DataInitializerService>();
            
            var pipeline = new DeckParsingPipelineBuilder<FileParserContext>()
                .Register(new DeckedBuilderCsvParser())
                .Build();

            services.AddSingleton(pipeline);

            return services;
        }
    }
}
