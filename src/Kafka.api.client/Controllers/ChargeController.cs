using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using System.Text;
using Kafka.api.client.Dtos;
using Newtonsoft.Json;

namespace Kafka.api.client.Controllers
{
    [Route("api/[controller]")]
    public class ChargeController : Controller
    {
        [HttpPost]
        public ResponseDto Create([FromBody] RequestDto request)
        {
            PushRequest(request);
            //Encrypt card number before pushing to stream.
            var response = new ResponseDto
            {
                Id = Guid.NewGuid().ToString(),
                CardNumber = request.CardNumber,
                Amount = request.Amount,
                Currency = request.Currency,
                ResponseCode = "10000"
            };   

            //Replace above dummy response with redis client connection.

            return response;
        }

        private void PushRequest(RequestDto request)
        {
            string kafkaEndpoint = "127.0.0.1:9092";
            //Create or use the ckotopic to push request for validation in the first stream.
            string kafkaTopic = "ckotopic";
            string jsonRequest = JsonConvert.SerializeObject(request);            
            var producerConfig = new Dictionary<string, object> { { "bootstrap.servers", kafkaEndpoint } };                     
            using (var producer = new Producer<Null, string>(producerConfig, null, new StringSerializer(Encoding.UTF8)))
            {
                Console.WriteLine("sednging kafka request");
                var result = producer.ProduceAsync(kafkaTopic, null, jsonRequest).GetAwaiter().GetResult();
                Console.WriteLine("sent kafka request");
            }
        }

    }
}
