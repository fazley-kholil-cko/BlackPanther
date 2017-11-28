using System;

namespace Checkout.BlackPanther.CardProcessing.Model
{
    /// <summary>
    /// The currency charged to a card during processing. Details as per https://en.wikipedia.org/wiki/ISO_4217
    /// </summary>
    public class Currency
    {
        public Currency(string iso3Code, string numericCode, int exponent)
        {
            if (string.IsNullOrEmpty(iso3Code)) throw new ArgumentNullException(nameof(iso3Code));
            if (string.IsNullOrEmpty(numericCode)) throw new ArgumentNullException(nameof(numericCode));
            if (exponent < 0 || exponent > 3) throw new ArgumentOutOfRangeException(nameof(exponent));

            Iso3Code = iso3Code;
            NumericCode = numericCode;
            Exponent = exponent;
        }

        /// <summary>
        /// The three-letter ISO currency code.
        /// </summary>
        /// <example>USD</example>
        public string Iso3Code { get; }

        /// <summary>
        /// Three-digit numeric currency code.
        /// </summary>
        /// <example>840 (USD)</example>
        public string NumericCode { get; }

        /// <summary>
        /// The relationship between the major currency unit and its corresponding minor currency uni assuming a base of 10.
        /// </summary>
        /// <example>
        /// USD is equal to 100 of its minor currency unit the "cent".
        /// So the USD has exponent 2 (10 to the power 2 is 100, which is the number of cents in a dollar)
        /// </example>
        public int Exponent { get; }
    }
}
