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
using Checkout.BlackPanther.ApiGW.Dtos;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace Checkout.BlackPanther.ApiGW.Controllers
{
    [Produces("application/json")]
    [Route("api/Transaction")]
    public class TransactionController : Controller
    {
        private readonly Dictionary<string, object> _producerConfig;
        private readonly string _kafkaEndpoint;
        private readonly string _inTopic;
        private static Producer<Null, string> _producer;
        private readonly AppSettings _appSettings;
        readonly ILogger<TransactionController> _logger;

        public TransactionController(IOptions<AppSettings> appSettings, ILogger<TransactionController> logger)
        {
            _appSettings = appSettings.Value;
            _logger = logger;
            _kafkaEndpoint =_appSettings.KafkaConnection;
            _inTopic = _appSettings.KafkaTopicName;
            _producerConfig = new Dictionary<string, object> { { "bootstrap.servers", _kafkaEndpoint } };  
            if (_producer == null)
                _producer = new Producer<Null, string>(_producerConfig, null, new StringSerializer(Encoding.UTF8));
        }

        [HttpPost]
        public async Task<object> Create([FromBody] TransactionRequest request)
        {
            _logger.LogInformation("Post message received.");
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

            var cache = RedisConnectorHelper.Connection.GetDatabase();
            RedisValue value = RedisValue.Null;
            while (value == RedisValue.Null)
                value = cache.StringGet(request.CorrelationId.ToString());

            return JsonConvert.DeserializeObject<ResponseDto>(value.ToString());
        }

        private async Task PushRequestAsync(TransactionRequest request)
        {
            Console.WriteLine("App setting:" + _appSettings.KafkaConnection);
            string jsonRequest = JsonConvert.SerializeObject(request);
            //Console.WriteLine("sending kafka request");
            long tickStart = Environment.TickCount;
            var result = _producer.ProduceAsync(_inTopic, null, jsonRequest).GetAwaiter().GetResult();            
            _logger.LogInformation($"API Gateway took {Environment.TickCount - tickStart} ms to produce to topic {_inTopic}");
            //Console.WriteLine("kafka request send");
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