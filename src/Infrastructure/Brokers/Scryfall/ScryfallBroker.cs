using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Contracts.IInfrastructure;
using Application.Exceptions;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Models.tmp.Scryfall;
using RESTFulSense.Exceptions;
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
            Set set;
            try {
                set = await _apiFactoryClient.GetContentAsync<Set>($"sets/{id}");
            }
            catch (HttpResponseInternalServerErrorException e) {
                throw new CommunicationException("Unable to load set data from Scryfall", e);
            }

            var relativePath = new Uri(set.SearchUri).PathAndQuery;
            var setCards = new List<Card>();


            while (relativePath != string.Empty) {
                SearchResult result;
                try {
                    result = await _apiFactoryClient.GetContentAsync<SearchResult>(relativePath);
                }
                catch (HttpResponseInternalServerErrorException e) {
                    throw new CommunicationException("Unable to load search data from Scryfall", e);
                }

                setCards.AddRange(result.Data);
                relativePath = result.HasMore ? new Uri(result.NextPage).PathAndQuery : string.Empty;
            }

            return _mapper.Map<IEnumerable<MagicCard>>(setCards);
        }
    }
}
