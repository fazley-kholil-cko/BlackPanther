using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using Kafka.Consumer.Dtos;
using Newtonsoft.Json;

namespace Kafka.Consumer
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
            string kafkaTopic = "ckotopic";           
            var consumerConfig = new Dictionary<string, object>
            {
                { "group.id", "myconsumer" },
                { "bootstrap.servers", kafkaEndpoint },
            };
            
            using (var consumer = new Consumer<Null, string>(consumerConfig, null, new StringDeserializer(Encoding.UTF8)))
            {
                // Subscribe to the OnMessage event
                consumer.OnMessage += (obj, msg) =>
                {
                    Console.WriteLine($"Received: {msg.Value}");
                    var request = JsonConvert.DeserializeObject<RequestDto>(msg.Value);
                        //Uncomment below after setting docker settings to allow auto create topics                                                
                        //PushResponse(outStreamId, message);
                        Console.WriteLine("Card number: " + request.CardNumber);
                };               
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
                    consumer.Poll(10000);
                }
            }
        }

        //Process and Push in final response stream
        static private void PushResponse(string outStreamId, string message)
        {
            // The Kafka endpoint address
            string kafkaEndpoint = "127.0.0.1:9092";
            //The stream which should be read by redis
            string kafkaTopic = "OutStream";
            // Create the producer configuration
            var producerConfig = new Dictionary<string, object> { { "bootstrap.servers", kafkaEndpoint } };
            var UniqueId = Guid.NewGuid().ToString();
            // Create the producer
            using (var producer = new Producer<Null, string>(producerConfig, null, new StringSerializer(Encoding.UTF8)))
            {
                var result = producer.ProduceAsync(kafkaTopic, null, (message + UniqueId)).GetAwaiter().GetResult();
                Console.WriteLine("Creating topic with message -> " + message);
            }
        }
    }
}
