namespace Bud.Service.UnitTests.Country
{
    using System.Collections.Generic;
    using Bud.Service.Country;
    using FluentValidation.TestHelper;
    using Xunit;

    public class CountryCodeValidatorTests
    {
        [Theory]
        [InlineData("GB")]
        [InlineData("GBR")]
        public void Validate_Success_ReturnsExpectedResult(string isoCode)
        {
            var validator = new CountryCodeValidator();

            var result = validator.TestValidate(isoCode);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [MemberData(nameof(ValidateFailureTestData))]
        public void Validate_Failure_ReturnsExpectedResult(string isoCode, string expectedError)
        {
            var validator = new CountryCodeValidator();

            var result = validator.TestValidate(isoCode);

            result.ShouldHaveValidationErrorFor(x => x).WithErrorMessage(expectedError);
        }

        public static IList<object[]> ValidateFailureTestData()
        {
            var lengthError = "ISO code must be 2 or 3 characters in length.";
            var emptyError = "ISO code must be provided.";

            return new List<object[]>
            {
                new object[] { "AB3", "ISO code must consist of letters only." },
                new object[] { "A", lengthError },
                new object[] { "ABCD", lengthError },
                new object[] { "", emptyError },
                new object[] { " ", emptyError },
            };
        }
    }
}