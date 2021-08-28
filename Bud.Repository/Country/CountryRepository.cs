namespace Bud.Repository.Country
{
    using System.Linq;
    using System.Threading.Tasks;
    using Bud.DTO;
    using Bud.Util.Serialization;
    using Flurl.Http;

    public class CountryRepository : ICountryRepository
    {
        private readonly IXmlSerializerWrapper<countries> _xmlSerializer;

        public CountryRepository(IXmlSerializerWrapper<countries> xmlSerializer)
        {
            _xmlSerializer = xmlSerializer;
        }

        public async Task<Country> GetCountry(string isoCode)
        {
            var apiResponse = await $"http://api.worldbank.org/v2/country/{isoCode}".GetAsync();
            var apiResponseStream = await apiResponse.ResponseMessage.Content.ReadAsStreamAsync();
            var xmlResponse = _xmlSerializer.Deserialize(apiResponseStream);

            if (xmlResponse?.country == null || !xmlResponse.country.Any())
            {
                return null;
            }

            var country = xmlResponse.country.First();

            return new Country
            {
                Name = country.name,
                Region = country.region?.FirstOrDefault()?.Value ?? string.Empty,
                CapitalCity = country.capitalCity,
                Latitude = decimal.TryParse(country.latitude, out var latitude) ? latitude : 0,
                Longitude = decimal.TryParse(country.longitude, out var longitude) ? longitude : 0
            };
        }
    }
}