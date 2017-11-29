using System;
using System.Collections.Generic;
using System.Text;

namespace Checkout.BlackPanther.ApiGW.Dtos
{
    public class ResponseDto
    {
        public Guid CorrelationId { get; set; }
        public string Id { get; set; }
        public DateTime Created { get; set; }
        public int Amount { get; set; }
        public string Currency { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public string ResponseAdvancedInfo { get; set; }
        public string Status { get; set; }
        public string AuthCode { get; set; }
        public long CardProcessingProcessAsync { get; set; }
        public long SimulatorProcessingTime { get; set; }

        public CardDetails Card { get; set; }

        public class CardDetails
        {
            public string Id { get; set; }
            public string ExpiryMonth { get; set; }
            public string ExpiryYear { get; set; }
            public string Last4 { get; set; }
            public string PaymentMethod { get; set; }
            public string Bin { get; set; }
            public string Type { get; set; }
            public string Category { get; set; }
            public string ProductId { get; set; }
            public string ProductType { get; set; }
            public string Issuer { get; set; }
            public string IssuerCountryIso2 { get; set; }
            public string Fingerprint { get; set; }
            public string Name { get; set; }
            public string CvvCheck { get; set; }
            public string AvsCheck { get; set; }
        }
    }
}
