namespace Bud.Service.Country
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Bud.DTO;
    using Bud.Repository.Country;
    using FluentValidation;

    public class CountryService : ICountryService
    {
        private readonly IValidator<string> _countryValidator;
        private readonly ICountryRepository _countryRepository;

        public CountryService(
            IValidator<string> countryValidator,
            ICountryRepository countryRepository)
        {
            _countryValidator = countryValidator;
            _countryRepository = countryRepository;
        }

        public async Task<Result<Country>> GetCountry(string isoCode)
        {
            var validationResult = _countryValidator.Validate(isoCode);
            if (!validationResult.IsValid)
            {
                return new Result<Country>
                {
                    Success = false,
                    Errors = validationResult.Errors.Select(e => e.ErrorMessage)
                };
            }

            var country = await _countryRepository.GetCountry(isoCode);

            if (country == null)
            {
                return new Result<Country>
                {
                    Success = false,
                    Errors = new List<string> { "Country not found" }
                };
            }

            return new Result<Country>
            {
                Success = true,
                Item = country
            };
        }
    }
}