using Checkout.BlackPanther.CardProcessing.Model;
using Checkout.BlackPanther.CardProcessing.Processor.Simulator;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Checkout.BlackPanther.CardProcessing
{
    class Program
    {
        //// The Kafka endpoint address
        const string KAFKA_ENDPOINT = "127.0.0.1:9092";
        const string CONSUMER_GROUP_ID = "checkout.cardprocessing";
        const string INPUT_TOPIC = "out_risk";

        const string OUTPUT_TOPIC = "out_response";

        static void Main(string[] args)
        {

            Console.WriteLine($"{nameof(Checkout.BlackPanther.CardProcessing)} console has started.");

            var consumerConfig = new Dictionary<string, object>
            {
                { "group.id", CONSUMER_GROUP_ID },
                { "bootstrap.servers", KAFKA_ENDPOINT },
            };

            using (var consumer = new Consumer<Null, string>(consumerConfig, null, new StringDeserializer(Encoding.UTF8)))
            {

                consumer.OnMessage += HandleMessage;
                consumer.Subscribe(new List<string>() { INPUT_TOPIC });

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

            //RequestDto req = new RequestDto
            //{
            //    currency = "840",
            //    cvv = "1051",
            //    number = "345678901234564",
            //    value = 100.00M,
            //    email = "test@cko.com",
            //    expiryMonth = "06",
            //    expiryYear = "2017",
            //    name = "Mozell",
            //    trackId = "100055"
            //};

            //createCharge(req).Wait();
            //Console.ReadLine();

        }

        private static void HandleMessage(object obj, Message<Null, string> msg)
        {
            Console.WriteLine($"Received: {msg.Value}");
            var request = JsonConvert.DeserializeObject<RequestDto>(msg.Value);

            //RequestDto req = new RequestDto
            //{
            //    currency = "840",
            //    cvv = "1051",
            //    number = "345678901234564",
            //    value = 100.00M,
            //    email = "test@cko.com",
            //    expiryMonth = "06",
            //    expiryYear = "2017",
            //    name = "Mozell",
            //    trackId = "100055"
            //};

            Task.Run(() => ProcessAsync(request));

        }

        public static async Task ProcessAsync(RequestDto req)
        {
            try
            {
                //Acquirer configuration details should be sent in request
                AcquirerConfiguration acqConfig = new AcquirerConfiguration(
                    1, //Acquirer ID  
                    "Simulator", // Acquirer Name
                    "http://localhost/WebService.ebs/service.asmx", // Acquirer uri
                    "8000001", // acquirer credentials merchant/terminal ID
                    "Password1!", // Password
                    null,
                    null,
                    null,
                    null);

                Currency cur = new Currency(
                    "USD", // Currency symbol + currency exponent shold be sent down in request 
                    req.currency,
                    2);

                Merchant merchant = new Merchant(1, 1000001, "6559", "London", "LDN", "GB", "GBR", "826", ""); // Merchant details should be sent down in request

                Card card = new Card(req.number,
                    int.Parse(req.expiryMonth),
                    int.Parse(req.expiryYear),
                    CardScheme.Visa, // Scheme should be included in request
                    req.cvv,
                    req.name, // Card holder name 
                    "FirstName",
                    "LastName", new
                    Address(
                        "address",
                        "",
                        "london",
                        "",
                        "12345", // compulsory to create address object
                        new Country(
                            "Great Britain",
                            "GB",
                            "GBR")));

                ChargeRequest charge = ChargeRequest.
                    CreateAuthoriseRequest(
                        cur,
                        req.value, // Amount
                        merchant, // Merchant details
                        false, // Is dynamic descriptor 
                        "test account for EBS", "London",  // billing descriptor
                        req.trackId, //transactionId Id
                        card, // Card details
                        null // correlationId
                        );

                CardProcessorRequest request = new CardProcessorRequest(long.Parse(req.trackId), acqConfig, charge);
                SimulatorCardProcessor simulatorCardProcessor = new SimulatorCardProcessor(new HttpClient());

                var processorResponse = await simulatorCardProcessor.ProcessAsync(request);
                //                public string AuthCode { get; set; } "923259"
                //public string AcquirerTransactionId { get; set; } "8123654135"
                //public string AcquirerReferenceNumber { get; set; } 000923259668
                //public string AcquirerResponse { get; set; } "APPROVED"
                //public string AcquirerResponseCode { get; set; } 00
                //public string AvsCheckResult { get; set; } S
                //public string CvvCheckResult { get; set; } Y
                //public Dictionary<string, string> AcquirerMetadata { get; set; }

                Dto.ResponseDto response = new Dto.ResponseDto();
                response.Id = req.trackId; //Should be the charge Id
                response.Created = request.ChargeRequest.RequestedOn;
                response.Amount = ConvertToMinorValue(request.ChargeRequest.Amount, request.ChargeRequest.Currency.Exponent);
                //response.ResponseCode = processorResponse.AcquirerResponseCode; // Nornally Acquirer response code mapping is done here
                //response.ResponseMessage = ""; // On AcquirerResponseCode we retrieve the response description
                //response.ResponseAdvancedInfo = ""; // On AcquirerResponseCode we retrieve the response long description
                if(processorResponse.AcquirerResponse == "APPROVED")
                {
                    response.ResponseCode = "10000";
                    response.ResponseMessage = "Approved";
                    response.ResponseAdvancedInfo = "";
                    response.Status = "Authorised";
                    response.AuthCode = processorResponse.AuthCode;
                }

                response.Card = new Dto.ResponseDto.CardDetails();
                response.Card.ExpiryMonth = request.ChargeRequest.Card.ExpiryMonth.ToString().PadLeft(2, '0');
                response.Card.ExpiryYear = request.ChargeRequest.Card.ExpiryYear.ToString().PadLeft(2, '0');
                response.Card.Last4 = request.ChargeRequest.Card.Number.Substring(request.ChargeRequest.Card.Number.Length - 4, 4);
                response.Card.Name = request.ChargeRequest.Card.CardholderName;
                response.Card.Bin = request.ChargeRequest.Card.Number.Substring(0, 8);
                response.Card.CvvCheck = processorResponse.CvvCheckResult;
                response.Card.AvsCheck = processorResponse.AvsCheckResult;

                // Send response to the out_response topic

                // Send to update transaction topic or is the insert/update of the transaction table to be done here?


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + Environment.NewLine + e.StackTrace);
                // Send to Log topic

                //Generate error response
            }
        }


        public static int ConvertToMinorValue(decimal majorValue, int currencyExponent)
        {
            return Convert.ToInt32(majorValue * (decimal)Math.Pow(10.0, currencyExponent));
        }

        private static void PushRequest(string serializedRequest, string outputTopic)
        {
            var producerConfig = new Dictionary<string, object> { { "bootstrap.servers", KAFKA_ENDPOINT } };

            using (var producer = new Producer<Null, string>(producerConfig, null, new StringSerializer(Encoding.UTF8)))
            {
                Console.WriteLine($"Producing response to {outputTopic}");
                var result = producer.ProduceAsync(outputTopic, null, (serializedRequest)).GetAwaiter().GetResult();
                System.Console.WriteLine($"Produced -> {serializedRequest}");
            }
        }

        private static void SendToOutTopic(Dto.ResponseDto response)
        {
            string serializedRequest = JsonConvert.SerializeObject(response);
            PushRequest(serializedRequest, OUTPUT_TOPIC);
        }


        public static async Task createCharge(RequestDto req)
        {
            try
            {
                AcquirerConfiguration acqConfig = new AcquirerConfiguration(1, "Simulator", "http://localhost/WebService.ebs/service.asmx", "8000001", "Password1!", null, null, null, null);

                Currency cur = new Currency("USD", "840", 2);
                Merchant merchant = new Merchant(1, 8000001, "6559", "London", "LDN", "GB", "GBR", "826", "");
                Card card = new Card(req.number, int.Parse(req.expiryMonth), int.Parse(req.expiryYear), CardScheme.Visa, req.cvv, req.name, "FirstName", "LastName", new Address("address", "", "london", "", "12345", new Country("Great Britain", "GB", "GBR")));
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
