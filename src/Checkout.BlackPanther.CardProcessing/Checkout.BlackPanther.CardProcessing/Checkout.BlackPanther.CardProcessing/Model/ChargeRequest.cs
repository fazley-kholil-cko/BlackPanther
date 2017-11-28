using System;
using System.Collections.Generic;


namespace Checkout.BlackPanther.CardProcessing.Model
{
    public class ChargeRequest
    {
        protected ChargeRequest(Currency currency,
            decimal amount,
            Merchant merchant,
            bool isDynamicDescriptor,
            string billingDescriptor,
            string billingDescriptorCity,
            string transactionId,
            string correlationId)
        {
            if (currency == null) throw new ArgumentNullException(nameof(currency));
            if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount));
            if (merchant == null) throw new ArgumentNullException(nameof(merchant));
            if (string.IsNullOrEmpty(transactionId)) throw new ArgumentNullException(nameof(transactionId));
            if (string.IsNullOrEmpty(billingDescriptor)) throw new ArgumentNullException(nameof(billingDescriptor));
            if (string.IsNullOrEmpty(billingDescriptorCity)) throw new ArgumentNullException(nameof(billingDescriptorCity));

            Currency = currency;
            Amount = amount;
            Merchant = merchant;
            IsDynamicDescriptor = isDynamicDescriptor;
            BillingDescriptor = billingDescriptor;
            BillingDescriptorCity = billingDescriptorCity;
            TransactionId = transactionId;
            CorrelationId = correlationId;

            RequestedOn = DateTime.UtcNow;
        }

        /// <summary>
        /// Gets the date/time that the charge was requested and subsequently sent to the processor
        /// </summary>
        public DateTime RequestedOn { get; }

        /// <summary>
        /// An action code that identifies the action to be performed against the card
        /// </summary>
        public GatewayActionCode ActionCode { get; private set; }

        /// <summary>
        /// Gets the curency of the request.
        /// </summary>
        public Currency Currency { get; }

        public decimal Amount { get; }

        /// <summary>
        /// Gets the merchant and business details
        /// </summary>
        public Merchant Merchant { get; }

        /// <summary>
        /// Gets whether this is a dynamic billing descriptor.
        /// </summary>
        public bool IsDynamicDescriptor { get; }

        /// <summary>
        /// Gets the billing descriptor, usually the trading name of the Merchant.
        /// https://en.wikipedia.org/wiki/Billing_descriptor
        /// </summary>
        public string BillingDescriptor { get; }

        /// <summary>
        /// Gets the billing descriptor city
        /// </summary>
        public string BillingDescriptorCity { get; }

        /// <summary>
        /// Gets the transaction Id of the originating system
        /// </summary>
        public string TransactionId { get; }

        /// <summary>
        /// The card to process
        /// </summary>
        public Card Card { get; private set; }

        /// <summary>
        /// Gets the charge ID (root transaction)
        /// </summary>
        public string ChargeId { get; private set; }

        /// <summary>
        /// Gets the acquirers original transaction identifier.
        /// </summary>
        public string AcquirerTransactionId { get; private set; }

        /// <summary>
        /// Gets the correlation identifier used to trace the request over multiple processes e.g. API Request ID
        /// </summary>
        public string CorrelationId { get; }

        /// <summary>
        /// The "UDF" metadata provided by the merchant.
        /// </summary>
        public Dictionary<string, string> Metadata { get; private set; }

        /// <summary>
        /// The Electronic-Commerce indicator required for 3DS
        /// </summary>
        public string Eci { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string Cavv { get; private set; }

        /// <summary>

        /// </summary>
        public string Xid { get; private set; }

        /// <summary>
        /// The Payer Authentication Response required for 3DS
        /// </summary>
        public string ParesStatus { get; private set; }

        public static ChargeRequest CreateAuthoriseRequest(Currency currency,
            decimal amount,
            Merchant merchant,
            bool isDynamicDescriptor,
            string billingDescriptor,
            string billingDescriptorCity,
            string transactionId,
            Card card,
            string correlationId,
            string eci = null,
            string cavv = null,
            string xid = null,
            string paresStatus = null
        )
        {
            if (card == null) throw new ArgumentNullException(nameof(card));

            return new ChargeRequest(
                currency,
                amount,
                merchant,
                isDynamicDescriptor,
                billingDescriptor,
                billingDescriptorCity,
                transactionId,
                correlationId
            )
            {
                ActionCode = GatewayActionCode.Authorise,
                Card = card
            };
        }

        public static ChargeRequest CreateCaptureRequest(
            Currency currency,
            decimal amount,
            Merchant merchant,
            bool isDynamicDescriptor,
            string billingDescriptor,
            string billingDescriptorCity,
            string transactionId,
            string chargeId,
            string acquirerTransactionId,
            string correlationId
        )
        {
            if (string.IsNullOrEmpty(chargeId)) throw new ArgumentNullException(nameof(chargeId));
            
            return new ChargeRequest(
                currency,
                amount,
                merchant,
                isDynamicDescriptor,
                billingDescriptor,
                billingDescriptorCity,
                transactionId,
                correlationId
            )
            {
                ActionCode = GatewayActionCode.Capture,
                AcquirerTransactionId = acquirerTransactionId,
                ChargeId = chargeId
            };
        }

        public static ChargeRequest CreateRefundRequest(
            Currency currency,
            decimal amount,
            Merchant merchant,
            bool isDynamicDescriptor,
            string billingDescriptor,
            string billingDescriptorCity,
            string transactionId,
            string chargeId,
            string acquirerTransactionId,
            string correlationId
        )
        {
            if (string.IsNullOrEmpty(chargeId)) throw new ArgumentNullException(nameof(chargeId));
            
            return new ChargeRequest(
                currency,
                amount,
                merchant,
                isDynamicDescriptor,
                billingDescriptor,
                billingDescriptorCity,
                transactionId,
                correlationId
            )
            {
                ActionCode = GatewayActionCode.Refund,
                AcquirerTransactionId = acquirerTransactionId,
                ChargeId = chargeId
            };
        }

    }
}