using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Infrastructure.Brokers.Scryfall;
using Infrastructure.Mapping;
using Infrastructure.Models.tmp.Scryfall;
using Moq;
using RESTFulSense.Exceptions;
using Tynamix.ObjectFiller;

namespace Infrastructure.Integration.Brokers.Scryfall {
    public partial class ScryfallBrokerTests {
        private static readonly Guid _setId = Guid.Parse("A6776181-3407-4915-BF3B-5BA617E8F363");
        private static readonly string _setCode = "4th";
        private static readonly string _baseUri = "https://api.sometestsource.com";

        private readonly IMapper _mapper;

        private readonly string _searchString =
            "{0}/cards/search?format=json&include_extras=false&include_multilingual=false&order=set&page={1}&q=e%3A{2}&unique=prints";

        private Mock<IScryfallFactoryClient> _restfulApiClientMock500Response;
        private Mock<IScryfallFactoryClient> _restfulApiClientMockEmptySetResponse;
        private Mock<IScryfallFactoryClient> _restfulApiClientMockFullResponse;

        public ScryfallBrokerTests() {
            SetupFullResponse();
            Setup500Response();
            SetupEmptySetResponse();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();
        }

        private void SetupEmptySetResponse() {
            _restfulApiClientMockEmptySetResponse = new Mock<IScryfallFactoryClient>();
            _restfulApiClientMockEmptySetResponse.Setup(
                client => client.GetContentAsync<Set>($"sets/{_setId}")).ReturnsAsync(GetSetResponse);
            _restfulApiClientMockEmptySetResponse.Setup(
                    client => client.GetContentAsync<SearchResult>(
                        $"/cards/search?order=set&q=e%3A{_setCode}&unique=prints"))
                .ReturnsAsync(new SearchResult {Data = new List<Card>()});
        }

        private void Setup500Response() {
            _restfulApiClientMock500Response = new Mock<IScryfallFactoryClient>();
            _restfulApiClientMock500Response.Setup(
                client => client.GetContentAsync<Set>($"sets/{_setId}")).ReturnsAsync(GetSetResponse);
            _restfulApiClientMock500Response.Setup(
                    client => client.GetContentAsync<SearchResult>(
                        $"/cards/search?order=set&q=e%3A{_setCode}&unique=prints"))
                .ThrowsAsync(new HttpResponseInternalServerErrorException(null, "Error"));
        }

        private void CreateSearchResult(int forPage) {
            _restfulApiClientMockFullResponse.Setup(
                    client => client.GetContentAsync<SearchResult>(
                        forPage == 1
                            ? $"/cards/search?order=set&q=e%3A{_setCode}&unique=prints"
                            : string.Format(_searchString, string.Empty, forPage, _setCode)))
                .ReturnsAsync(GetSearchResults(forPage));
        }

        private Set GetSetResponse() {
            return new() {
                Name = "Fourth Edition",
                Code = _setCode,
                Id = _setId.ToString(),
                Uri = $"{_baseUri}/sets/{_setId}",
                SearchUri = $"{_baseUri}/cards/search?order=set&q=e%3A{_setCode}&unique=prints"
            };
        }

        private SearchResult GetSearchResults(int forPage) {
            return new() {
                Data = GetCardResponse(10),
                HasMore = forPage < 4,
                NextPage = string.Format(_searchString, _baseUri, ++forPage, _setCode)
            };
        }

        private List<Card> GetCardResponse(int count) {
            return new Filler<Card>().Create(count).ToList();
        }

        private void SetupFullResponse() {
            _restfulApiClientMockFullResponse = new Mock<IScryfallFactoryClient>();
            _restfulApiClientMockFullResponse.Setup(
                client => client.GetContentAsync<Set>($"sets/{_setId}")).ReturnsAsync(GetSetResponse());
            CreateSearchResult(1);
            CreateSearchResult(2);
            CreateSearchResult(3);
            CreateSearchResult(4);
        }
    }
}
