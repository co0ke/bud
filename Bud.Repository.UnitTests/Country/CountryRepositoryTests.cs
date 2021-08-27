namespace Bud.Repository.UnitTests.Country
{
    using System;
    using System.Threading.Tasks;
    using Bud.DTO;
    using Bud.Repository.Country;
    using FluentAssertions;
    using Flurl.Http.Testing;
    using Xunit;

    public class CountryRepositoryTests
    {
        private const string SuccessResponse =
            "<wb:countries xmlns:wb=\"http://www.worldbank.org\" page=\"1\" pages=\"1\" per_page=\"50\" total=\"1\"><wb:country id=\"BRA\"><wb:iso2Code>BR</wb:iso2Code><wb:name>Brazil</wb:name><wb:region id=\"LCN\" iso2code=\"ZJ\">Latin America & Caribbean </wb:region><wb:adminregion id=\"LAC\" iso2code=\"XJ\">Latin America & Caribbean (excluding high income)</wb:adminregion><wb:incomeLevel id=\"UMC\" iso2code=\"XT\">Upper middle income</wb:incomeLevel><wb:lendingType id=\"IBD\" iso2code=\"XF\">IBRD</wb:lendingType><wb:capitalCity>Brasilia</wb:capitalCity><wb:longitude>-47.9292</wb:longitude><wb:latitude>-15.7801</wb:latitude></wb:country></wb:countries>";

        private const string NotFoundResponse = @"<wb:error xmlns:wb=""http://www.worldbank.org""><wb:message id=""120"" key=""Invalid value"">The provided parameter value is not valid</wb:message></wb:error>";

        [Fact(Skip = "Incomplete and failing")]
        public async Task GetCountry_CountryFound_ReturnsExpectedResult()
        {
            // Arrange
            var repository = new CountryRepository();

            // Act
            Country country;

            using (var httpTest = new HttpTest())
            {
                httpTest.RespondWith(SuccessResponse);
                country = await repository.GetCountry("BRA");
            };

            // Assert
            country.Should().BeEquivalentTo(new Country
            {
                Name = "Brazil",
                Region = "Latin America & Caribbean",
                CapitalCity = "Brasilia",
                Latitude = -15.7801M,
                Longitude = -47.9292M
            });
        }

        [Fact(Skip = "Incomplete and failing")]
        public void GetCountry_NoCountryFound_ThrowsInvalidOperationException()
        {
            // Arrange
            var repository = new CountryRepository();

            // Act
            Action call = () =>
            {
                using (var httpTest = new HttpTest())
                {
                    httpTest.RespondWith(NotFoundResponse);
                    repository.GetCountry("XX");
                };
            };

            // Assert
            call.Should().Throw<InvalidOperationException>();
        }
    }
}