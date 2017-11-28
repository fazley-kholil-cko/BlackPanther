namespace Checkout.BlackPanther.CardProcessing.Model
{
    /// <summary>
    /// Represents normalised response code information returned by the Gateway
    /// </summary>
    public class GatewayResponseDetails
    {
        public string ResponseCode { get; set; }
        public string ResponseDescription { get; set; }
        public string ResponseLongDescription { get; set; }
        public string AvsCode { get; set; }
        public string AvsCodeDescription { get; set; }
        public string CvvCode { get; set; }
        public string CvvCodeDescription { get; set; }
    }
}
