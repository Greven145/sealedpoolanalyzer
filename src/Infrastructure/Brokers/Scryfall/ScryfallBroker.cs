﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Contracts.IInfrastructure;
using Application.Exceptions;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Models.Scryfall;
using RESTFulSense.Exceptions;
using Set = Domain.Entities.Set;

namespace Infrastructure.Brokers.Scryfall {
    public class ScryfallBroker : ISetLoader {
        private readonly IScryfallFactoryClient _apiFactoryClient;
        private readonly IMapper _mapper;

        public ScryfallBroker(IScryfallFactoryClient apiFactoryClient, IMapper mapper) {
            _apiFactoryClient = apiFactoryClient ?? throw new ArgumentNullException(nameof(apiFactoryClient));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async ValueTask<Set> GetSetFromId(Guid id) {
            Models.Scryfall.Set scryfallSet;
            try {
                scryfallSet = await _apiFactoryClient.GetContentAsync<Models.Scryfall.Set>($"sets/{id}");
            }
            catch (HttpResponseInternalServerErrorException e) {
                throw new CommunicationException("Unable to load set data from Scryfall", e);
            }


            scryfallSet.SearchUri = scryfallSet.SearchUri.Replace($"q=e%3A{scryfallSet.Code}", $"q=e:{scryfallSet.Code}+-is:themepack");

            var relativePath = new Uri(scryfallSet.SearchUri).PathAndQuery;
            var setCards = new List<Card>();


            while (relativePath != string.Empty) {
                await Task.Delay(TimeSpan.FromMilliseconds(250)); //Try to be good citizen on public APIs
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
            
            var magicCards = _mapper.Map<ICollection<MagicCard>>(setCards);
            var set = _mapper.Map<Set>(scryfallSet);
            set.MagicCards = magicCards;
            return set;
        }
    }
}
