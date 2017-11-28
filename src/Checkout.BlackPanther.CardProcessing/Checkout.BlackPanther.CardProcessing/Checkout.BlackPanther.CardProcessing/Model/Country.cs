using System;

namespace Checkout.BlackPanther.CardProcessing.Model
{
    public class Country
    {
        public Country(string name, string iso2Code, string iso3Code)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrEmpty(iso2Code)) throw new ArgumentNullException(nameof(iso2Code));
            if (string.IsNullOrEmpty(iso3Code)) throw new ArgumentNullException(nameof(iso3Code));

            Name = name;
            Iso2Code = iso2Code;
            Iso3Code = iso3Code;
        }

        public string Name { get; }
        public string Iso2Code { get; }
        public string Iso3Code { get; }
    }
}
