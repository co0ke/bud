namespace Bud.Repository.UnitTests.Country
{
    using System.IO;
    using System.Threading.Tasks;
    using Bud.DTO;
    using Bud.Repository.Country;
    using Bud.Util.Serialization;
    using FluentAssertions;
    using Flurl.Http.Testing;
    using Moq;
    using Xunit;

    public class CountryRepositoryTests
    {
        private readonly Mock<IXmlSerializerWrapper<countries>> _xmlSerializer;

        public CountryRepositoryTests()
        {
            _xmlSerializer = new Mock<IXmlSerializerWrapper<countries>>(MockBehavior.Strict);
        }

        [Fact]
        public async Task GetCountry_CountryFound_ReturnsExpectedResult()
        {
            // Arrange
            var repository = new CountryRepository(_xmlSerializer.Object);

            var countriesXml = new countries
            {
                country = new []
                {
                    new countriesCountry
                    {
                        name = "Name",
                        region = new []{ new countriesCountryRegion { Value = "Region" } },
                        capitalCity = "Capital City",
                        latitude = "1.13",
                        longitude = "-2.04"
                    }
                }
            };

            _xmlSerializer
                .Setup(x => x.Deserialize(It.IsAny<Stream>()))
                .Returns(countriesXml);

            // Act
            Country country;

            using (var httpTest = new HttpTest())
            {
                httpTest.RespondWith("<data></data>");
                country = await repository.GetCountry("BRA");
            };

            // Assert
            country.Should().BeEquivalentTo(new Country
            {
                Name = "Name",
                Region = "Region",
                CapitalCity = "Capital City",
                Latitude = 1.13M,
                Longitude = -2.04M
            });
        }

        [Fact]
        public async Task GetCountry_CountryFoundWithBadData_ReturnsFallbackValues()
        {
            // Arrange
            var repository = new CountryRepository(_xmlSerializer.Object);

            var countriesXml = new countries
            {
                country = new []
                {
                    new countriesCountry
                    {
                        name = "Name",
                        capitalCity = "CapitalCity",
                        region = System.Array.Empty<countriesCountryRegion>(),
                        latitude = "1....13",
                        longitude = "invalid"
                    }
                }
            };

            _xmlSerializer
                .Setup(x => x.Deserialize(It.IsAny<Stream>()))
                .Returns(countriesXml);

            // Act
            Country country;

            using (var httpTest = new HttpTest())
            {
                httpTest.RespondWith("<data></data>");
                country = await repository.GetCountry("BRA");
            };

            // Assert
            country.Should().BeEquivalentTo(new Country
            {
                Name = "Name",
                CapitalCity = "CapitalCity",
                Region = "",
                Latitude = 0,
                Longitude = 0
            });
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task GetCountry_NoCountryInModel_ReturnsNull(bool useNull)
        {
            // Arrange
            var repository = new CountryRepository(_xmlSerializer.Object);

            var countriesXml = new countries
            {
                country = useNull ? null : System.Array.Empty<countriesCountry>()
            };

            _xmlSerializer
                .Setup(x => x.Deserialize(It.IsAny<Stream>()))
                .Returns(countriesXml);

            Country country;

            // Act
            using (var httpTest = new HttpTest())
            {
                httpTest.RespondWith("<data></data>");
                country = await repository.GetCountry("XX");
            };

            // Assert
            country.Should().BeNull();
        }
    }
}