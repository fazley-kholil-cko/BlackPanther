using System.Xml.Serialization;

namespace Checkout.BlackPanther.CardProcessing.Processor.Simulator
{
    [XmlRoot("processTransactionDs", Namespace = "")]
    public class SimulatorRequest
    {
        [XmlElement("SubEntityID", IsNullable = true)]
        public string SubEntityId { get; set; }

        [XmlElement("EntityID", IsNullable = true)]
        public string EntityId { get; set; }

        [XmlElement("ProcessID", IsNullable = true)]
        public string ProcessId { get; set; }

        [XmlElement(IsNullable = true)]
        public string ProcessPassword { get; set; }

        [XmlElement("serviceURL", IsNullable = true)]
        public string ServiceUrl { get; set; }

        [XmlElement("trackID", IsNullable = true)]
        public string TrackId { get; set; }

        [XmlElement("OriginalTransactionID", IsNullable = true)]
        public string OriginalTransactionId { get; set; }

        [XmlElement("OriginalMaskedTransactionID", IsNullable = true)]
        public string OriginalMaskedTransactionId { get; set; }

        [XmlElement]
        public string Amount { get; set; }

        [XmlElement]
        public string CardNumber { get; set; }

        [XmlElement(IsNullable = true)]
        public string Cvv2 { get; set; }

        [XmlElement("ExpDay", IsNullable = true)]
        public string ExpiryDay { get; set; }

        [XmlElement("ExpMonth", IsNullable = true)]
        public string ExpiryMonth { get; set; }

        [XmlElement("ExpYear", IsNullable = true)]
        public string ExpiryYear { get; set; }

        [XmlElement("member", IsNullable = true)]
        public string Member { get; set; }

        [XmlElement("address", IsNullable = true)]
        public string BillingAddress { get; set; }

        [XmlElement("bill_address2", IsNullable = true)]
        public string BillingAddress2 { get; set; }

        [XmlElement("bill_city", IsNullable = true)]
        public string BillingCity { get; set; }

        [XmlElement("bill_state", IsNullable = true)]
        public string BillingState { get; set; }

        [XmlElement("bill_country", IsNullable = true)]
        public string BillingCountryIso2Code { get; set; }

        [XmlElement("bill_countryISO3code", IsNullable = true)]
        public string BillingCountryIso3Code { get; set; }

        [XmlElement("bill_email", IsNullable = true)]
        public string BillingEmail { get; set; }

        [XmlElement("zip", IsNullable = true)]
        public string ZipCode { get; set; }

        /// <summary>
        /// Acquirer Action Code (from ActionCodeMapping)
        /// </summary>
        [XmlElement("actionCode")]
        public int ActionCode { get; set; }

        [XmlElement("TransactionID", IsNullable = true)]
        public string TransactionId { get; set; }

        [XmlElement("ProcessResponseCode", IsNullable = true)]
        public string ResponseCode { get; set; }

        [XmlElement("ProcessErrorCode", IsNullable = true)]
        public string ErrorCode { get; set; }

        [XmlElement("currencyCode")]
        public string CurrencyCode { get; set; }

        [XmlElement("currencyExponent", IsNullable = true)]
        public int? CurrencyExponent { get; set; }

        [XmlElement("referenceId", IsNullable = true)]
        public string ReferenceId { get; set; }

        [XmlElement(IsNullable = true)]
        public string AuthCode { get; set; }

        [XmlElement("countrycode", IsNullable = true)]
        public string CountryCode { get; set; }

        [XmlElement("subEntityUser", IsNullable = true)]
        public string SubEntityUser { get; set; }

        [XmlElement("subEntityPassword", IsNullable = true)]
        public string SubEntityPassword { get; set; }

        [XmlElement("subEntityAction", IsNullable = true)]
        public int? SubEntityAction { get; set; }

        [XmlElement("customerIp", IsNullable = true)]
        public string CustomerIp { get; set; }

        [XmlElement("merchantIp", IsNullable = true)]
        public string MerchantIp { get; set; }

        [XmlElement("udf1", IsNullable = true)]
        public string Udf1 { get; set; }

        [XmlElement("udf2", IsNullable = true)]
        public string Udf2 { get; set; }

        [XmlElement("udf3", IsNullable = true)]
        public string Udf3 { get; set; }

        [XmlElement("udf4", IsNullable = true)]
        public string Udf4 { get; set; }

        [XmlElement("udf5", IsNullable = true)]
        public string Udf5 { get; set; }

        [XmlElement("currency", IsNullable = true)]
        public string Currency { get; set; }

        [XmlElement("ResponseCodeID", IsNullable = true)]
        public string ResponseCodeId { get; set; }

        [XmlElement("ErrorCodeID", IsNullable = true)]
        public string ErrorCodeId { get; set; }

        [XmlElement("TransactionDate", IsNullable = true)]
        public string TransactionDate { get; set; }

        [XmlElement("result", IsNullable = true)]
        public string Result { get; set; }

        [XmlElement("paymentID", IsNullable = true)]
        public string PaymentId { get; set; }

        [XmlElement("SubEntityOriginalTransactionID", IsNullable = true)]
        public string SubEntityOriginalTransactionId { get; set; }

        [XmlElement("subEntityCode", IsNullable = true)]
        public string SubEntityCode { get; set; }

        [XmlElement(IsNullable = true)]
        public string Token { get; set; }

        [XmlElement(IsNullable = true)]
        public string Sender { get; set; }

        [XmlElement(IsNullable = true)]
        public string Channel { get; set; }

        [XmlElement(IsNullable = true)]
        public string Error { get; set; }

        [XmlElement("subEntityResponse", IsNullable = true)]
        public string SubEntityResponse { get; set; }

        [XmlElement("Pareq", IsNullable = true)]
        public string PaReq { get; set; }

        [XmlElement(IsNullable = true)]
        public string TermUrl { get; set; }

        [XmlElement(IsNullable = true)]
        public string Md { get; set; }

        [XmlElement("acsURL", IsNullable = true)]
        public string AcsUrl { get; set; }

        [XmlElement("_3D", IsNullable = true)]
        public string ThreeD { get; set; }

        [XmlElement("advanceduser", IsNullable = true)]
        public string AdvancedUser { get; set; }

        [XmlElement("advancedpassword", IsNullable = true)]
        public string AdvancedPassword { get; set; }

        [XmlElement("avs_check", IsNullable = true)]
        public string AvsCheck { get; set; }

        [XmlElement(IsNullable = true)]
        public string SubEntityProcessCode { get; set; }

        [XmlElement(IsNullable = true)]
        public string SubEntityPosInfo { get; set; }

        [XmlElement("SubEntityCvv2_check", IsNullable = true)]
        public string SubEntityCvv2Check { get; set; }

        [XmlElement("POSCountryCode", IsNullable = true)]
        public string PosCountryCode { get; set; }

        [XmlElement("MCC", IsNullable = true)]
        public string Mcc { get; set; }

        [XmlElement(IsNullable = true)]
        public string EntityCountryCode { get; set; }

        [XmlElement("MerchantID", IsNullable = true)]
        public string MerchantId { get; set; }

        [XmlElement(IsNullable = true)]
        public string CardType { get; set; }

        [XmlElement(IsNullable = true)]
        public string RecurringStartDate { get; set; }

        [XmlElement(IsNullable = true)]
        public string RecurringInterval { get; set; }

        [XmlElement(IsNullable = true)]
        public string RecurringIntervalType { get; set; }

        [XmlElement(IsNullable = true)]
        public string TransactionSource { get; set; }

        [XmlElement(IsNullable = true)]
        public string ServiceType { get; set; }

        [XmlElement(IsNullable = true)]
        public string CustomerToken { get; set; }

        /// <summary>
        /// Electronic Commerce Indicator
        /// </summary>
        [XmlElement("eci", IsNullable = true)]
        public string Eci { get; set; }

        [XmlElement("xid", IsNullable = true)]
        public string Xid { get; set; }

        [XmlElement("cavv", IsNullable = true)]
        public string Cavv { get; set; }

        [XmlElement(IsNullable = true)]
        public string ParesStatus {get;set;}

        [XmlElement("merchantcustomerid", IsNullable = true)]
        public string MerchantCustomerId { get; set; }

        [XmlElement("parenttransactionid", IsNullable = true)]
        public string ParentTransactionId { get; set; }

        [XmlElement("acceptorname", IsNullable = true)]
        public string AcceptorName { get; set; }

        [XmlElement("acceptorcity", IsNullable = true)]
        public string AcceptorCity { get; set; }

        [XmlElement("acceptorcountry", IsNullable = true)]
        public string AcceptorCountry { get; set; }

        [XmlElement("billingdescriptor", IsNullable = true)]
        public string BillingDescriptor { get; set; }
    }
}
