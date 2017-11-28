using System;
using System.Collections.Generic;
using System.Text;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using Newtonsoft.Json;
using Request.Validator.Dtos;
using Valit;

namespace Request.Validator
{
    class Program
    {
        static void Main(string[] args)
        {
            ValidateRequestFromKafka();
        }

        public static void ValidateRequestFromKafka()
        {

            // The Kafka endpoint address
            var kafkaEndpoint = "127.0.0.1:9092";

            //Validator topic
            var inKafkaTopic = "in_api_requests";
            var outKafkaTopic = "out_api_requests";
            var outErrorKafkaTopic = "out_response";
            
            var consumerConfig = new Dictionary<string, object>
            {
                { "group.id", "valdator.consumer" },
                { "bootstrap.servers", kafkaEndpoint },
            };

            using (var consumer = new Consumer<Null, string>(consumerConfig, null, new StringDeserializer(Encoding.UTF8)))
            {
                // Subscribe to the OnMessage event
                consumer.OnMessage += (obj, msg) =>
                {
                    var request = JsonConvert.DeserializeObject<RequestDto>(msg.Value);
                    var result = ValidateModel(request);

                    if (result.Succeeded)
                    {
                        Console.WriteLine($"Valid Request Pushing to {outKafkaTopic}");
                        PushResponse(outKafkaTopic, Guid.NewGuid().ToString(), JsonConvert.SerializeObject(request));
                    }
                    else
                    {
                        Console.WriteLine($"InValid Request Pushing to {outErrorKafkaTopic}");
                        var error = new ErrorDto {Message = "Validation Error: Currency is not valid"};
                        PushResponse(outErrorKafkaTopic, Guid.NewGuid().ToString(), JsonConvert.SerializeObject(error));
                    }

                };
                consumer.Subscribe(new List<string> { inKafkaTopic });

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

        public static IValitResult ValidateModel(RequestDto model)
        {
            var result = ValitRules<RequestDto>
                .Create()
                .Ensure(m => m.Currency.ToUpper(), _ => _
                    .IsEqualTo("USD"))
                .For(model)
                .Validate();

            return result;
        }

        public static void PushResponse(string outStreamTopic, string outStreamId, string message)
        {
            // The Kafka endpoint address
            string kafkaEndpoint = "127.0.0.1:9092";
            //The stream which should be read by redis
            // Create the producer configuration
            var producerConfig = new Dictionary<string, object> { { "bootstrap.servers", kafkaEndpoint } };
            var UniqueId = Guid.NewGuid().ToString();
            // Create the producer
            using (var producer = new Producer<Null, string>(producerConfig, null, new StringSerializer(Encoding.UTF8)))
            {
                var result = producer.ProduceAsync(outStreamTopic, null, (message)).GetAwaiter().GetResult();
                Console.WriteLine("Creating topic with message -> " + message);
            }
        }

    }
}
