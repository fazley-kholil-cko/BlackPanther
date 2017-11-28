using System;
using System.Collections.Generic;
using System.Text;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using Newtonsoft.Json;

namespace Kafka.Risk
{
    class Program
    {
        const string kafkaEndpoint = "127.0.0.1:9092";
        const string consumerGroupId = "checkout.risk";
        const string inputTopic = "out_api_requests";
        const string outputTopic = "out_risk";
        const string finalTopic = "out_response";


        static void Main(string[] args)
        {
            Console.WriteLine($"{nameof(Kafka.Risk)} console has started.");

            try
            {
                ProcessRequest();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        static private void ProcessRequest()
        {
            var consumerConfig = new Dictionary<string, object>
            {
                { "group.id", consumerGroupId },
                { "bootstrap.servers", kafkaEndpoint },
            };
            
            using (var consumer = new Consumer<Null, string>(consumerConfig, null, new StringDeserializer(Encoding.UTF8)))
            {
                // Subscribe to the OnMessage event
                consumer.OnMessage += HandleMessage;       
                consumer.Subscribe(new List<string>() { inputTopic });   

                var cancelled = false;

                Console.CancelKeyPress += (_, e) =>
                {
                    e.Cancel = true;
                        cancelled = true;
                };

                Console.WriteLine("Ctrl-C to exit.");

                // Poll for messages
                while (!cancelled)
                {
                    consumer.Poll(10000);
                }
            }
        }

        private static void HandleMessage(object obj, Message<Null, string> msg)
        {
            Console.WriteLine($"Received: {msg.Value}");
            var request = JsonConvert.DeserializeObject<Request>(msg.Value);

            var blacklistCheckResponse = CheckCardBlacklist(request);
  
            if (blacklistCheckResponse.HasRisks)
                SendToFinalOutput(blacklistCheckResponse.RiskMessage);
            else 
                SendToNext(request);
        }

        private static RiskResponse CheckCardBlacklist(Request request)
        {
            const string blacklistedCard = "4242424242424242";

            if (request.Number.Equals(blacklistedCard))
                return RiskResponse.Fail("Card blacklisted");

            return RiskResponse.Ok();
        }

        private static void PushRequest(string serializedRequest, string outputTopic)
        {         
            var producerConfig = new Dictionary<string, object> { { "bootstrap.servers", kafkaEndpoint } };

            using (var producer = new Producer<Null, string>(producerConfig, null, new StringSerializer(Encoding.UTF8)))
            {
                Console.WriteLine($"Producing response to {outputTopic}");
                producer.ProduceAsync(outputTopic, null, serializedRequest);
                System.Console.WriteLine($"Produced -> {serializedRequest}");
            }
        }

        private static void SendToNext(Request request)
        {
            string serializedRequest = JsonConvert.SerializeObject(request);
            PushRequest(serializedRequest, outputTopic);
        }

        private static void SendToFinalOutput(RiskMessage riskMessage)
        {
            string serializedRequest = JsonConvert.SerializeObject(riskMessage);
            PushRequest(serializedRequest, finalTopic);
        }
    }
}
