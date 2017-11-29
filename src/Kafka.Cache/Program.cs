using System;
using System.Collections.Generic;
using System.Text;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using Newtonsoft.Json;

namespace Kafka.Cache
{
    class Program
    {
        static void Main(string[] args)
        {
            ProcessRequest();
        }

        static private void ProcessRequest()
        {
            // The Kafka endpoint address
            string kafkaEndpoint = "127.0.0.1:9092";

            //Validator topic
            string kafkaTopic = "out_response";

            var consumerConfig = new Dictionary<string, object>
            {
                { "group.id", "checkout.cache" },
                { "bootstrap.servers", kafkaEndpoint },
            };
            
            using (var consumer = new Consumer<Null, string>(consumerConfig, null, new StringDeserializer(Encoding.UTF8)))
            {
                // Subscribe to the OnMessage event
                consumer.OnMessage += HandleMessage;

                consumer.Subscribe(new List<string>() { kafkaTopic });                
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
                    consumer.Poll(100);
                }
            }
        }

        private static void HandleMessage(object obj, Message<Null, string> msg)
        {
            var cache = RedisConnectorHelper.Connection.GetDatabase();
            var request = JsonConvert.DeserializeObject<Request>(msg.Value);
            cache.StringSet(request.CorrelationId.ToString(), msg.Value);
            Console.WriteLine(msg.Value);
            Console.WriteLine($"Wrote into cache - {request.CorrelationId}");
        }
    }
}