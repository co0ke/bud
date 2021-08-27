namespace Bud.Repository.Country
{
    using System.Threading.Tasks;
    using Bud.DTO;

    public interface ICountryRepository
    {
        Task<Country> GetCountry(string isoCode);
    }
}