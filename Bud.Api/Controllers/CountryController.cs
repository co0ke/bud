namespace Bud.Api.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using Bud.DTO;
    using Microsoft.AspNetCore.Mvc;
    using Bud.Service.Country;
    using FluentValidation;

    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ICountryService _countryService;
        private readonly IValidator<string> _countryCodeValidator;

        public CountryController(
            ICountryService countryService,
            IValidator<string> countryCodeValidator)
        {
            _countryService = countryService;
            _countryCodeValidator = countryCodeValidator;
        }

        [HttpGet]
        [Route("countries/{isoCode}")]
        public async Task<IActionResult> Get(string isoCode)
        {
            var validationResult = _countryCodeValidator.Validate(isoCode);
            if (!validationResult.IsValid)
            {
                var errorResult = new Result<Country>
                {
                    Success = false,
                    Errors = validationResult.Errors.Select(x => x.ErrorMessage)
                };
                return UnprocessableEntity(errorResult);
            }

            var result = await _countryService.GetCountry(isoCode);
            return new ObjectResult(result);
        }
    }
}
