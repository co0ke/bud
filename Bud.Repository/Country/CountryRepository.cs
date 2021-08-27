namespace Bud.Repository.Country
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Xml.Serialization;
    using Bud.DTO;
    using Flurl.Http;

    public class CountryRepository : ICountryRepository
    {
        public async Task<Country> GetCountry(string isoCode)
        {
            var apiResponse = await $"http://api.worldbank.org/v2/country/{isoCode}".GetAsync();
            var apiResponseStream = await apiResponse.ResponseMessage.Content.ReadAsStreamAsync();

            var xmlSerializer = new XmlSerializer(typeof(countries));
            var xmlResult = (countries)xmlSerializer.Deserialize(apiResponseStream);

            if (xmlResult?.country == null || !xmlResult.country.Any())
            {
                return null;
            }

            var country = xmlResult.country.First();

            return new Country
            {
                Name = country.name,
                Region = country.region.First()?.Value ?? string.Empty,
                CapitalCity = country.capitalCity,
                Latitude = decimal.TryParse(country.latitude, out var latitude) ? latitude : 0,
                Longitude = decimal.TryParse(country.longitude, out var longitude) ? longitude : 0
            };
        }
    }
}