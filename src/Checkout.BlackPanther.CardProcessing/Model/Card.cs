using System;

namespace Checkout.BlackPanther.CardProcessing.Model
{
    public class Card
    {
        public Card(
            string number, 
            int expiryMonth, 
            int expiryYear, 
            CardScheme scheme,
            string cvv = null,
            string cardholderName = null,
            string cardholderFirstName = null,
            string cardholderLastName = null, 
            Address billingAddress = null)
        {
            if (string.IsNullOrEmpty(number)) throw new ArgumentNullException(nameof(number));
            if (expiryMonth < 1 || expiryMonth > 12) throw new ArgumentOutOfRangeException(nameof(expiryMonth));

            Number = number;
            ExpiryMonth = expiryMonth;
            ExpiryYear = expiryYear;
            Scheme = scheme;

            Cvv = cvv;
            CardholderName = cardholderName;
            CardholderFirstName = cardholderFirstName;
            CardholderLastName = cardholderLastName;
            BillingAddress = billingAddress;
        }

        public string CardholderName { get; }
        public string CardholderFirstName { get; }
        public string CardholderLastName { get; }
        public string Number { get; }
        public string Cvv { get; }
        public int ExpiryMonth { get; }
        public int ExpiryYear { get; }
        public CardScheme Scheme { get; }
        public Address BillingAddress { get; }
    }
}
