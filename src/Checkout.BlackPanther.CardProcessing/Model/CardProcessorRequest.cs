using System;
using Checkout.BlackPanther.CardProcessing.Model;

namespace Checkout.BlackPanther.CardProcessing.Model
{
    public class CardProcessorRequest
    {
        public CardProcessorRequest()
        {

        }

        public CardProcessorRequest(
            long internalTransactionId,
            AcquirerConfiguration acquirerConfiguration,
            ChargeRequest chargeRequest)
        {
            if (acquirerConfiguration == null) throw new ArgumentNullException(nameof(acquirerConfiguration));
            if (chargeRequest == null) throw new ArgumentNullException(nameof(chargeRequest));

            InternalTransactionId = internalTransactionId;
            AcquirerConfiguration = acquirerConfiguration;
            ChargeRequest = chargeRequest;
        }

        public long InternalTransactionId { get; }
        public AcquirerConfiguration AcquirerConfiguration { get; } 
        public ChargeRequest ChargeRequest { get; }
    }
}
