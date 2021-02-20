using System;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Application.Exceptions;
using Infrastructure.Contracts;
using Infrastructure.Models.DeckedBuilder;

namespace Infrastructure.Brokers.DeckedBuilder {
    public class DeckedBuilderBroker : IDeckedBuilderLoader {
        private readonly IDeckedBuilderFactoryClient _apiClientFactory;
        private readonly IHttpClientFactory _httpClientFactory;
        private const string _latestPath = "latest-db-updates/latest.json";

        public DeckedBuilderBroker(IDeckedBuilderFactoryClient apiClientFactory, IHttpClientFactory httpClientFactory) {
            _apiClientFactory = apiClientFactory ?? throw new ArgumentNullException(nameof(apiClientFactory));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public ValueTask<DeckedBuilderLatest> GetLatest() =>
            _apiClientFactory.GetContentAsync<DeckedBuilderLatest>(_latestPath);

        public async ValueTask<Stream> DownloadDatabase(Uri url) => await _httpClientFactory.CreateClient().GetStreamAsync(url);
    }
}
