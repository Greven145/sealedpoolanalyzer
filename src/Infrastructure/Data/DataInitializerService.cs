using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistence;

namespace Infrastructure.Data {
    public class DataInitializerService : IHostedService {
        private readonly IServiceProvider _serviceProvider;

        public DataInitializerService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken) {
            using var scope = _serviceProvider.CreateScope();
            
            var context = scope.ServiceProvider.GetRequiredService<AnalyzerContext>();
            var dataLoader = scope.ServiceProvider.GetRequiredService<DataLoader>();

            //Do the migration asynchronously
            await context.Database.MigrateAsync(cancellationToken: cancellationToken);
            await dataLoader.EnsureSetDataIsUpToDate(cancellationToken);
            await dataLoader.EnsureReviewDataIsUpToDate(cancellationToken);
            await dataLoader.EnsureDeckedBuilderDataIsUpToDate(cancellationToken);
            await dataLoader.EnsureDeckedBuilderToScryfallRelationshipsUpToDate(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken) {
            // No op
            return Task.CompletedTask;
        }
    }
}
