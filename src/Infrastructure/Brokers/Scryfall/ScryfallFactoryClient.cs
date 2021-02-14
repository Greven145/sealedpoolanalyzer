using System;
using System.Net.Http;
using RESTFulSense.Clients;

namespace Infrastructure.Brokers.Scryfall {
    public class ScryfallFactoryClient : RESTFulApiFactoryClient, IScryfallFactoryClient {
        public ScryfallFactoryClient(HttpClient httpClient) : base(httpClient) {
            httpClient.BaseAddress = new Uri("https://api.scryfall.com");
        }
    }
}
