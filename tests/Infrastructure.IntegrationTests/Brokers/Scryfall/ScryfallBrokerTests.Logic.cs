using System.Linq;
using System.Threading.Tasks;
using Application.Exceptions;
using FluentAssertions;
using Infrastructure.Brokers.Scryfall;
using Xunit;

namespace Infrastructure.Integration.Brokers.Scryfall {
    public partial class ScryfallBrokerTests {
        [Fact]
        public void When500ResponseIsReturned_ApplicationExceptionIsThrown() {
            var broker = new ScryfallBroker(_restfulApiClientMock500Response.Object, _mapper);

            FluentActions.Invoking(async () => await broker.GetSetFromId(_setId)).Should()
                .Throw<CommunicationException>();
        }

        [Fact]
        public async Task WhenNoSetDataIsLoaded_EmptyListIsReturned() {
            var broker = new ScryfallBroker(_restfulApiClientMockEmptySetResponse.Object, _mapper);

            var result = await broker.GetSetFromId(_setId);

            result.Count().Should().Be(0);
        }

        [Fact]
        public async Task WhenSetDataIsLoaded_CardListIsReturned() {
            var broker = new ScryfallBroker(_restfulApiClientMockFullResponse.Object, _mapper);

            var result = await broker.GetSetFromId(_setId);

            result.Count().Should().Be(40);
        }
    }
}
