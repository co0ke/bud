namespace Bud.Service.Country
{
    using System.Threading.Tasks;
    using Bud.DTO;

    public interface ICountryService
    {
        Task<Result<Country>> GetCountry(string isoCode);
    }
}