namespace Kafka.Risk
{
    public class Request
    {
        public string CardNumber { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
    }
}