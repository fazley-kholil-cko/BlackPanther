namespace Kafka.Risk
{
    public class RiskMessage
    {
        public string Message { get; }

        public RiskMessage(string message)
        {
            Message = message;
        }
    }
}