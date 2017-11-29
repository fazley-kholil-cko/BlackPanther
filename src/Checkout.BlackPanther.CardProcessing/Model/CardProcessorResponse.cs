using System.Collections.Generic;

namespace Checkout.BlackPanther.CardProcessing.Processors
{
    public class CardProcessorResponse
    {
        public string AuthCode { get; set; }
        public string AcquirerTransactionId { get; set; }
        public string AcquirerReferenceNumber { get; set; }
        public string AcquirerResponse { get; set; }
        public string AcquirerResponseCode { get; set; }
        public string AvsCheckResult { get; set; }
        public string CvvCheckResult { get; set; }
        public Dictionary<string, string> AcquirerMetadata {get;set;}
    }
}
