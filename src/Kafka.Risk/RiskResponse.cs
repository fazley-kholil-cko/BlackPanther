using System.Collections.Generic;

namespace Kafka.Risk
{
    public class RiskResponse
    {
        public RiskMessage RiskMessage { get; set; }
        public bool HasRisks => RiskMessage != null;

        public static RiskResponse Ok()
        {
            return new RiskResponse();
        }

        public static RiskResponse Fail(string riskMessage)
        {
            return new RiskResponse
            {
                RiskMessage = new RiskMessage(riskMessage)
            };
        }
    }
}