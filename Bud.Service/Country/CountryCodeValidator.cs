namespace Bud.Service.Country
{
    using System;
    using System.Linq;
    using FluentValidation;

    public class CountryCodeValidator : AbstractValidator<string>
    {
        private const int MinLength = 2;
        private const int MaxLength = 3;

        public CountryCodeValidator()
        {
            RuleFor(isoCode => isoCode).NotEmpty()
                .WithMessage("ISO code must be provided.");

            RuleFor(isoCode => isoCode).Length(MinLength, MaxLength)
                .WithMessage("ISO code must be 2 or 3 characters in length.");

            RuleFor(isoCode => isoCode).Must(isoCode => isoCode.All(Char.IsLetter))
                .WithMessage("ISO code must consist of letters only.");
        }
    }
}