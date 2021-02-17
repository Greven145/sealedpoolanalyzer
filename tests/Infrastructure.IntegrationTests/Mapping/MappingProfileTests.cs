using System;
using AutoMapper;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Mapping;
using Infrastructure.Models.tmp.Scryfall;
using Xunit;
using Set = Domain.Entities.Set;

namespace Infrastructure.Integration.Mapping {
    public class MappingProfileTests {
        private readonly MapperConfiguration _configuration;
        private readonly IMapper _mapper;

        public MappingProfileTests() {
            _configuration = new MapperConfiguration(cfg => cfg.AddProfile(typeof(MappingProfile)));
            _mapper = _configuration.CreateMapper();
        }

        [Fact]
        public void MappingProfile_IsValid() {
            _configuration.AssertConfigurationIsValid();
        }

        [Theory]
        [InlineData(typeof(Card), typeof(MagicCard))]
        [InlineData(typeof(Models.tmp.Scryfall.Set), typeof(Set))]
        public void TypesMapCorrectly(Type source, Type destination) {
            var instance = Activator.CreateInstance(source);
            FluentActions.Invoking(() => _mapper.Map(instance, source, destination)).Should()
                .NotThrow();
        }
    }
}
