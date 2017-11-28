using System.Xml.Serialization;

namespace Checkout.BlackPanther.CardProcessing.Processor.Simulator
{
    [XmlRoot("processTransactionDs", Namespace = "")]
    public class SimulatorResponse
    {
        [XmlElement("SubEntityID")]
        public string SubEntityId { get; set; }

        [XmlElement("EntityID")]
        public string EntityId { get; set; }

        [XmlElement("ProcessID")]
        public string ProcessId { get; set; }

        
        public string ProcessPassword { get; set; }

        [XmlElement("serviceURL")]
        public string ServiceUrl { get; set; }

        [XmlElement("trackID")]
        public string TrackId { get; set; }

        [XmlElement("OriginalTransactionID")]
        public string OriginalTransactionId { get; set; }

        public decimal Amount { get; set; }
      
        public string CardNumber { get; set; }
        
        public string Cvv2 { get; set; }

        [XmlElement("ExpDay")]
        public string ExpiryDay { get; set; }

        [XmlElement("ExpMonth")]
        public string ExpiryMonth { get; set; }

        [XmlElement("ExpYear")]
        public string ExpiryYear { get; set; }

        [XmlElement("member")]
        public string Member { get; set; }

        [XmlElement("address")]
        public string Address { get; set; }

        [XmlElement("zip")]
        public string ZipCode { get; set; }

        [XmlElement("actionCode")]
        public int ActionCode { get; set; }

        [XmlElement("TransactionID")]
        public string TransactionId { get; set; }

        [XmlElement("ProcessResponseCode")]
        public string ResponseCode { get; set; }

        [XmlElement("ProcessErrorCode")]
        public string ErrorCode { get; set; }

        [XmlElement("currencyCode")]
        public int CurrencyCode { get; set; }

        [XmlElement("referenceId")]
        public string ReferenceId { get; set; }

        public string AuthCode { get; set; }

        [XmlElement("countrycode")]
        public string CountryCode { get; set; }

        [XmlElement("subEntityUser")]
        public string SubEntityUser { get; set; }

        [XmlElement("subEntityPassword")]
        public string SubEntityPassword { get; set; }

        [XmlElement("subEntityAction")]
        public int SubEntityAction { get; set; }

        [XmlElement("customerIp")]
        public string CustomerIp { get; set; }

        [XmlElement("merchantIp")]
        public string MerchantIp { get; set; }

        [XmlElement("udf1")]
        public string Udf1 { get; set; }

        [XmlElement("udf2")]
        public string Udf2 { get; set; }

        [XmlElement("udf3")]
        public string Udf3 { get; set; }

        [XmlElement("udf4")]
        public string Udf4 { get; set; }

        [XmlElement("udf5")]
        public string Udf5 { get; set; }

        [XmlElement("currency")]
        public string Currency { get; set; }

        [XmlElement("ResponseCodeID")]
        public string ResponseCodeId { get; set; }

        [XmlElement("ErrorCodeID")]
        public string ErrorCodeId { get; set; }

        [XmlElement("TransactionDate")]
        public string TransactionDate { get; set; }

        [XmlElement("result")]
        public string Result { get; set; }

        [XmlElement("paymentID")]
        public string PaymentId { get; set; }

        [XmlElement("SubEntityOriginalTransactionID")]
        public string SubEntityOriginalTransactionId { get; set; }

        [XmlElement("subEntityCode")]
        public string SubEntityCode { get; set; }

        public string Token { get; set; }

        public string Sender { get; set; }
        
        public string Channel { get; set; }
        
        public string Error { get; set; }

        [XmlElement("subEntityResponse")]
        public string SubEntityResponse { get; set; }

        [XmlElement("avs_check")]
        public string AvsCheck { get; set; }
    }
}
