using System;
using System.Net.Http;
using RESTFulSense.Clients;

namespace Infrastructure.Brokers.DeckedBuilder {
    public class DeckedBuilderFactoryClient : RESTFulApiFactoryClient, IDeckedBuilderFactoryClient {
        public DeckedBuilderFactoryClient(HttpClient httpClient) : base(httpClient) {
            httpClient.BaseAddress = new Uri("https://images.deckedbuilder.com");
        }
    }
}
