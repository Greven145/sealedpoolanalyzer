using Application.Contracts.IPersistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Contracts;
using Persistence.Repositories;

namespace Persistence {
    public static class DependencyInjectionExtensions {
        public static IServiceCollection AddPersistence(this IServiceCollection services) {
            services.AddDbContext<AnalyzerContext>();
            services.AddDbContext<DeckedBuilderContext>();

            services.AddScoped<ICardRepository, CardRepository>();
            services.AddScoped<ISetRepository, SetRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IDeckedBuilderRepository, DeckedBuilderRepository>();
            services.AddScoped<IDb2ScryfallRepository, Db2ScrylfallRepository>();

            return services;
        }
    }
}
