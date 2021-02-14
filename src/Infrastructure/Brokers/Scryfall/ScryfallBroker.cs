using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Contracts.IInfrastructure;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Models.tmp.Scryfall;
using Set = Infrastructure.Models.tmp.Scryfall.Set;

namespace Infrastructure.Brokers.Scryfall {
    public class ScryfallBroker : ISetLoader {
        private readonly IScryfallFactoryClient _apiFactoryClient;
        private readonly IMapper _mapper;

        public ScryfallBroker(IScryfallFactoryClient apiFactoryClient, IMapper mapper) {
            _apiFactoryClient = apiFactoryClient ?? throw new ArgumentNullException(nameof(apiFactoryClient));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async ValueTask<IEnumerable<MagicCard>> GetSetFromId(Guid id) {
            var set = await _apiFactoryClient.GetContentAsync<Set>($"sets/{id}");
            var relativePath = new Uri(set.SearchUri).PathAndQuery;
            var setCards = new List<Card>();


            while (relativePath != string.Empty) {
                var result = await _apiFactoryClient.GetContentAsync<SearchResult>(relativePath);
                setCards.AddRange(result.Data);
                relativePath = result.HasMore ? new Uri(result.NextPage).PathAndQuery : string.Empty;
            }

            return _mapper.Map<IEnumerable<MagicCard>>(setCards);
        }
    }
}
