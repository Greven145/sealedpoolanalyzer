using Application.Contracts.IPersistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Repositories;

namespace Persistence {
    public static class DependencyInjectionExtensions {
        public static IServiceCollection AddPersistence(this IServiceCollection services) {
            services.AddDbContext<AnalyzerContext>();

            services.AddScoped<ICardRepository, CardRepository>();
            services.AddScoped<ISetRepository, SetRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();

            return services;
        }
    }
}
