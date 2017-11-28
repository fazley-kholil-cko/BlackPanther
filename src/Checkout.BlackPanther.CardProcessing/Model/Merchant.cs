namespace Checkout.BlackPanther.CardProcessing.Model
{
    public class Merchant
    {
        public Merchant(
            int id,
            int businessId,
            string categoryCode,
            string city,
            string cityCode,
            string countryIso2Code,
            string countryIso3Code,
            string countryNumericCode,
            string postalCode
        )
        {
            Id = id;
            BusinessId = businessId;
            CategoryCode = categoryCode;
            City = city;
            CityCode = cityCode;
            CountryIso2Code = countryIso2Code;
            CountryIso3Code = countryIso3Code;
            CountryNumericCode = countryNumericCode;
            PostalCode = postalCode;
        }

        /// <summary>
        /// Gets the Checkout Merchant identifier.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Gets the merchant business identifier.
        /// </summary>
        public int BusinessId {get;}

        /// <summary>
        /// Gets the Merchant Category Code (MCC)
        /// https://en.wikipedia.org/wiki/Merchant_category_code
        /// </summary>
        public string CategoryCode { get; }

        public string City { get; }

        public string CityCode { get; }

        /// <summary>
        /// Gets trading country Iso2 code of the Merchant e.g. US.
        /// </summary>
        public string CountryIso2Code { get; }

        public string CountryIso3Code { get; }

        ///
        /// Gets the country's ISO numeric code
        ///
        public string CountryNumericCode { get; }

        public string PostalCode { get; }
    }
}