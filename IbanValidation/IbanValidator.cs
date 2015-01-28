﻿using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using System.Text.RegularExpressions;

namespace IbanValidation
{
    public class IbanValidator : IIbanValidator
    {
        public IbanValidationResult Validate(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) { return IbanValidationResult.ValueMissing; }

            if (value.Length < 2) { return IbanValidationResult.ValueTooSmall; }

            var countryCode = value.Substring(0, 2).ToUpper();

            int lengthForCountryCode;

            var countryCodeKnown = _lengths.TryGetValue(countryCode, out lengthForCountryCode);
            if (!countryCodeKnown)
            {
                return IbanValidationResult.CountryCodeNotKnown;
            }

            // Check length.
            if (value.Length < lengthForCountryCode) { return IbanValidationResult.ValueTooSmall; }
            if (value.Length > lengthForCountryCode) { return IbanValidationResult.ValueTooBig; }

            var upperValue = value.ToUpper();
            var newIban = upperValue.Substring(4) + upperValue.Substring(0, 4);

            newIban = Regex.Replace(newIban, @"\D", match => (match.Value[0] - 55).ToString(CultureInfo.InvariantCulture));

            var remainder = BigInteger.Parse(newIban) % 97;

            if (remainder != 1) { return IbanValidationResult.ValueFailsModule97Check; }

            return IbanValidationResult.IsValid;
        }

        private static readonly Dictionary<string, int> _lengths = new Dictionary<string, int>
        {
            {"AL", 28},
            {"AD", 24},
            {"AT", 20},
            {"AZ", 28},
            {"BE", 16},
            {"BH", 22},
            {"BA", 20},
            {"BR", 29},
            {"BG", 22},
            {"CR", 21},
            {"HR", 21},
            {"CY", 28},
            {"CZ", 24},
            {"DK", 18},
            {"DO", 28},
            {"EE", 20},
            {"FO", 18},
            {"FI", 18},
            {"FR", 27},
            {"GE", 22},
            {"DE", 22},
            {"GI", 23},
            {"GR", 27},
            {"GL", 18},
            {"GT", 28},
            {"HU", 28},
            {"IS", 26},
            {"IE", 22},
            {"IL", 23},
            {"IT", 27},
            {"KZ", 20},
            {"KW", 30},
            {"LV", 21},
            {"LB", 28},
            {"LI", 21},
            {"LT", 20},
            {"LU", 20},
            {"MK", 19},
            {"MT", 31},
            {"MR", 27},
            {"MU", 30},
            {"MC", 27},
            {"MD", 24},
            {"ME", 22},
            {"NL", 18},
            {"NO", 15},
            {"PK", 24},
            {"PS", 29},
            {"PL", 28},
            {"PT", 25},
            {"RO", 24},
            {"SM", 27},
            {"SA", 24},
            {"RS", 22},
            {"SK", 24},
            {"SI", 19},
            {"ES", 24},
            {"SE", 24},
            {"CH", 21},
            {"TN", 24},
            {"TR", 26},
            {"AE", 23},
            {"GB", 22},
            {"VG", 24}
        };
    }
}