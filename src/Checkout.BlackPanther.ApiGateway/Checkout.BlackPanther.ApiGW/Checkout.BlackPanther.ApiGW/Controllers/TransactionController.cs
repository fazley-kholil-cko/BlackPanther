using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Checkout.BlackPanther.ApiGW.Data;
using Checkout.BlackPanther.ApiGW.Data.Model;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Checkout.BlackPanther.ApiGW.Controllers
{
    [Produces("application/json")]
    [Route("api/Transaction")]
    public class TransactionController : Controller
    {
        private readonly Dictionary<string, object> _producerConfig;
        private readonly Dictionary<string, object> _consumerConfig;
        private readonly string _kafkaEndpoint;
        private readonly string _inTopic;
        private readonly string _outTopic;


        public TransactionController()
        {
            _kafkaEndpoint = "127.0.0.1:9092";
            _inTopic = "in_api_requests";
            _outTopic = "fakeredis";
            _producerConfig = new Dictionary<string, object> { { "bootstrap.servers", _kafkaEndpoint } };
            _consumerConfig = new Dictionary<string, object>
            {
                { "group.id", "myconsumer" },
                { "bootstrap.servers", _kafkaEndpoint },
            };
        }

        [HttpPost]
        public async Task<object> Create([FromBody] TransactionRequest request)
        {
            request.CorrelationId = Guid.NewGuid();

            if (request == null)
            {
                return BadRequest();
            }

            var validator = new TransactionRequestValidator();
            var results = validator.Validate(request);

            if (!results.IsValid)
            {
                return BadRequest(DisplayErrors(results.Errors));
            }

            //push to kafka
            PushRequestAsync(request);

        //     return await Task.Run(() =>
        //    {
        //        WaitForResponseAsync(request);

        //    }).ContinueWith((r) => Ok(r));

            var cache = RedisConnectorHelper.Connection.GetDatabase(); 

            RedisValue value = RedisValue.Null;

            while (value == RedisValue.Null)
                value = cache.StringGet(request.CorrelationId.ToString());

            return JsonConvert.DeserializeObject<TransactionRequest>(value.ToString());
        }



        private void WaitForResponseAsync(TransactionRequest request)
        {
            try
            {

                object response = null;

                using (var consumer = new Consumer<Null, string>(_consumerConfig, null, new StringDeserializer(Encoding.UTF8)))
                {
                    // Subscribe to the OnMessage event
                    consumer.OnMessage += (obj, msg) =>
                    {
                        Console.WriteLine($"Received: {msg.Value}");
                        response = JsonConvert.DeserializeObject<TransactionRequest>(msg.Value);
                    };
                    consumer.Subscribe(new List<string>() { _outTopic });
                    var cancelled = false;

                    // Poll for messages
                    while (!cancelled)
                    {
                        consumer.Poll(10000);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


        }

        private async Task PushRequestAsync(TransactionRequest request)
        {

            string jsonRequest = JsonConvert.SerializeObject(request);

            using (var producer = new Producer<Null, string>(_producerConfig, null, new StringSerializer(Encoding.UTF8)))
            {
                Console.WriteLine("sending kafka request");
                var result = producer.ProduceAsync(_inTopic, null, jsonRequest).GetAwaiter().GetResult();
                Console.WriteLine("kafka request send");
            }
        }

        private List<CustomError> DisplayErrors(IList<ValidationFailure> errors)
        {
            var list = new List<CustomError>();

            foreach (var failure in errors)
            {
                list.Add(new CustomError { Property = failure.PropertyName, ErrorMessage = failure.ErrorMessage });
            }

            return list;
        }


    }


}