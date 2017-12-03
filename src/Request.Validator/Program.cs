using System;
using System.Collections.Generic;
using System.Text;
using Serilog;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using Newtonsoft.Json;
using Request.Validator.Dtos;
using Valit;
using StructureMap;

namespace Request.Validator
{
    class Program
    {
        private static Producer<Null, string> _producer;
        private static IContainer _smContainer;
        private static ILogger _logger;
        public static IContainer SmContainer
        {
            get
            {
                if (_smContainer == null)
                {
                    _smContainer = new Container(config =>
                    {
                        config.AddRegistry<LoggingRegistry>();
                    });
                }
                _smContainer.AssertConfigurationIsValid();
                return _smContainer;
            }
        }


        private static void ResolveDependencies()
        {
            _logger = SmContainer.GetInstance<ILogger>();            

        }
        static void Main(string[] args)
        {
            ResolveDependencies();
            ValidateRequestFromKafka();
        }

        public static void ValidateRequestFromKafka()
        {

            // The Kafka endpoint address
            var kafkaEndpoint = AppSettings.KafkaConnection;//"127.0.0.1:9092";
            Console.WriteLine($"Kafka endpoint {kafkaEndpoint}");
            //Validator topic
            var inKafkaTopic = AppSettings.KafkaInTopicName;//"in_api_requests";
            var outKafkaTopic = AppSettings.KafkaOutTopicName;//"out_api_requests";
            var outErrorKafkaTopic = AppSettings.kafkaOutErrorTopicName;//"out_response";

            var consumerConfig = new Dictionary<string, object>
            {
                { "group.id", AppSettings.KafkaGroupName },
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
                        PushResponse(outKafkaTopic,
                        //Guid.NewGuid().ToString(), 
                        JsonConvert.SerializeObject(request));
                    }
                    else
                    {
                        Console.WriteLine($"InValid Request Pushing to {outErrorKafkaTopic}");
                        var error = new ErrorDto { Message = "Validation Error: Currency is not valid" };
                        PushResponse(outErrorKafkaTopic,
                        //Guid.NewGuid().ToString(), 
                        JsonConvert.SerializeObject(error));
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
                    consumer.Poll(100);
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

        public static void PushResponse(string outStreamTopic,
        //string outStreamId, 
        string message)
        {
            // The Kafka endpoint address
            string kafkaEndpoint = "127.0.0.1:9092";
            //The stream which should be read by redis
            // Create the producer configuration
            var producerConfig = new Dictionary<string, object> { { "bootstrap.servers", kafkaEndpoint } };
            //var UniqueId = Guid.NewGuid().ToString();
            if (_producer == null)
                _producer = new Producer<Null, string>(producerConfig, null, new StringSerializer(Encoding.UTF8));
            long tickStart = Environment.TickCount;
            var result = _producer.ProduceAsync(outStreamTopic, null, (message)).GetAwaiter().GetResult();

            _logger.Information($"Request Validator took {Environment.TickCount - tickStart} ms to produce to topic {outStreamTopic}");
        }

    }
}
