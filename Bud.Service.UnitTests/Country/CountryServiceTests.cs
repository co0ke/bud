namespace Bud.Service.UnitTests.Country
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Bud.DTO;
    using Bud.Repository.Country;
    using Bud.Service.Country;
    using FluentAssertions;
    using FluentValidation;
    using FluentValidation.Results;
    using Moq;
    using Xunit;

    public class CountryServiceTests
    {
        private readonly Mock<IValidator<string>> _validator;
        private readonly Mock<ICountryRepository> _repository;
        private readonly ICountryService _service;

        public CountryServiceTests()
        {
            _validator = new Mock<IValidator<string>>(MockBehavior.Strict);
            _repository  = new Mock<ICountryRepository>(MockBehavior.Strict);
            _service = new CountryService(_validator.Object, _repository.Object);
        }

        [Fact]
        public async Task GetCountry_ValidationFails_ReturnsExpectedResult()
        {
            // Arrange
            var isoCode = "XX";

            var validationResult = new ValidationResult(new List<ValidationFailure>
            {
                new ("property name", "Something went wrong"),
                new ("property name", "Something else went wrong"),
            });

            _validator
                .Setup(x => x.Validate(isoCode))
                .Returns(validationResult);

            // Act
            var result = await _service.GetCountry(isoCode);

            // Assert
            result.Should().BeEquivalentTo(new Result<Country>
            {
                Success = false,
                Errors = new List<string>
                {
                    "Something went wrong",
                    "Something else went wrong",
                }
            });
        }

        [Fact]
        public async Task GetCountry_CountryNotFound_ReturnsExpectedResult()
        {
            // Arrange
            var isoCode = "XX";

            _validator
                .Setup(x => x.Validate(isoCode))
                .Returns(new ValidationResult());

            _repository
                .Setup(x => x.GetCountry(isoCode))
                .ReturnsAsync((Country)null);

            // Act
            var result = await _service.GetCountry(isoCode);

            // Assert
            result.Should().BeEquivalentTo(new Result<Country>
            {
                Success = false,
                Errors = new List<string> { "Country not found" }
            });
        }

        [Fact]
        public async Task GetCountry_CountryFound_ReturnsExpectedResult()
        {
            // Arrange
            var isoCode = "GB";

            _validator
                .Setup(x => x.Validate(isoCode))
                .Returns(new ValidationResult());

            var country = new Country();
            _repository
                .Setup(x => x.GetCountry(isoCode))
                .ReturnsAsync(country);

            // Act
            var result = await _service.GetCountry(isoCode);

            // Assert
            result.Success.Should().BeTrue();
            result.Item.Should().BeSameAs(country);
            result.Errors.Should().BeNull();
        }
    }
}