using System;

namespace Checkout.BlackPanther.CardProcessing.Model
{
    public class Address
    {
        public Address(
            string line1,
            string line2,
            string townCity,
            string state,
            string zipCode,
            Country country)
        {
            if (country == null) throw new ArgumentNullException(nameof(country));
            if (string.IsNullOrEmpty(line1)) throw new ArgumentNullException(nameof(line1));
            if (string.IsNullOrEmpty(townCity)) throw new ArgumentNullException(nameof(townCity));
            if (string.IsNullOrEmpty(zipCode)) throw new ArgumentNullException(nameof(zipCode));

            Line1 = line1;
            Line2 = line2;
            TownCity = townCity;
            State = state;
            ZipCode = zipCode;
            Country = country;
        }

        public string Line1 { get; }
        public string Line2 { get; }
        public string TownCity { get; }
        public string State { get; }
        public string ZipCode { get; }
        public Country Country { get; }
    }
}
