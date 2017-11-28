using Checkout.BlackPanther.CardProcessing.Model;
using Checkout.BlackPanther.CardProcessing.Processor.Simulator;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Checkout.BlackPanther.CardProcessing
{
    class Program
    {
        static void Main(string[] args)
        {
            //// The Kafka endpoint address
            //string kafkaEndpoint = "127.0.0.1:9092";
            ////Validator topic
            //string kafkaTopic = "out_risky";
            //var consumerConfig = new Dictionary<string, object>
            //{
            //    { "group.id", "myconsumer" },
            //    { "bootstrap.servers", kafkaEndpoint },
            //};

            //using (var consumer = new Consumer<Null, string>(consumerConfig, null, new StringDeserializer(Encoding.UTF8)))
            //{
            //    // Subscribe to the OnMessage event
            //    consumer.OnMessage += (obj, msg) =>
            //    {
            //        Console.WriteLine($"Received: {msg.Value}");
            //        var request = JsonConvert.DeserializeObject<RequestDto>(msg.Value);
            //        //Uncomment below after setting docker settings to allow auto create topics                                                
            //        //PushResponse(outStreamId, message);



            //        Console.WriteLine("Card number: " + request.number);
            //    };
            //    consumer.Subscribe(new List<string>() { kafkaTopic });
            //    var cancelled = false;
            //    Console.CancelKeyPress += (_, e) =>
            //    {
            //        e.Cancel = true;
            //        cancelled = true;
            //    };
            //    Console.WriteLine("Ctrl-C to exit.");
            //    // Poll for messages
            //    while (!cancelled)
            //    {
            //        consumer.Poll(10000);
            //    }
            //}

            RequestDto req = new RequestDto
            {
                currency = "840",
                cvv = "1051",
                number = "345678901234564",
                value = 100.00M,
                email = "test@cko.com",
                expiryMonth = "06",
                expiryYear = "2017",
                name = "Mozell",
                trackId = "100055"
            };

            createCharge(req).Wait();
            Console.ReadLine();
        }

        public static async Task createCharge(RequestDto req)
        {
            try
            {
                AcquirerConfiguration acqConfig = new AcquirerConfiguration(1, "Simulator", "http://localhost/WebService.ebs/service.asmx", "8000001", "Password1!", null, null, null, null);

                Currency cur = new Currency("USD", "840", 2);
                Merchant merchant = new Merchant(1, 8000001, "6559", "London", "LDN", "GB", "GBR", "826", "");
                Card card = new Card(req.number, int.Parse(req.expiryMonth), int.Parse(req.expiryYear), CardScheme.Visa, req.cvv, req.name, "FirstName", "LastName", new Address("address", "", "london","", "12345", new Country("Great Britain", "GB", "GBR")));
                ChargeRequest charge = ChargeRequest.
                    CreateAuthoriseRequest(cur, req.value, merchant, false, "test account for EBS", "London", req.trackId, card, null);

                CardProcessorRequest request = new CardProcessorRequest(long.Parse(req.trackId), acqConfig, charge);

                SimulatorCardProcessor sim = new SimulatorCardProcessor(new HttpClient());

                var response = await sim.ProcessAsync(request);

                Console.WriteLine("Ctrl-C to exit.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + Environment.NewLine + e.StackTrace);
            }

        }
    }
}
