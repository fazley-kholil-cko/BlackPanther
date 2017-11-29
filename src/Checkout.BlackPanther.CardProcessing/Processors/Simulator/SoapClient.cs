using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Checkout.BlackPanther.CardProcessing.Processors.Simulator
{
    /// <summary>
    /// A Soap Client used for legacy processors such as the Simulator and Mashreq (WCF service)
    /// </summary>
    public class SoapClient
    {
        private const string XmlMediaType = "text/xml";
        private static readonly XNamespace SimulatorNamespace = "http://tempuri.org/";

        private readonly SoapClientOptions _options;
        private readonly HttpClient _httpClient;

        public SoapClient(SoapClientOptions options, HttpClient httpClient)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));

            _options = options;
            _httpClient = httpClient;
        }

        public async Task<SimulatorResponse> ProcessAsync(SimulatorRequest simulatorRequest)
        {
            if (simulatorRequest == null) throw new ArgumentNullException(nameof(simulatorRequest));

            var soapRequest = CreateSoapRequest(simulatorRequest);

            var request = new HttpRequestMessage(HttpMethod.Post, _options.Uri)
            {
                Content = new StringContent(soapRequest, Encoding.UTF8, XmlMediaType)
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(XmlMediaType));
            request.Content.Headers.ContentType = new MediaTypeHeaderValue(XmlMediaType);
            request.Headers.Add("SOAPAction", $"{SimulatorNamespace}process");

            var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            return await ProcessSoapResponse(response).ConfigureAwait(false);
        }

     
        private static string CreateSoapRequest(SimulatorRequest simulatorRequest)
        {
            XNamespace ns = "http://schemas.xmlsoap.org/soap/envelope/";
            XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
            XNamespace xsd = "http://www.w3.org/2001/XMLSchema";

            var simulatorXml = Serialize(simulatorRequest);

            var soapDocument = new XDocument(
                new XDeclaration("1.0", "UTF-8", "no"),
                new XElement(ns + "Envelope",
                    new XAttribute(XNamespace.Xmlns + "xsi", xsi),
                    new XAttribute(XNamespace.Xmlns + "xsd", xsd),
                    new XAttribute(XNamespace.Xmlns + "soap", ns),
                    new XElement(ns + "Body",
                        new XElement(SimulatorNamespace + "process",
                            new XElement(SimulatorNamespace + "param", simulatorXml)
                        )
                    )
                ));

            return soapDocument.ToString(SaveOptions.DisableFormatting);
        }

        private static async Task<SimulatorResponse> ProcessSoapResponse(HttpResponseMessage httpResponse)
        {
            var responseStream = await httpResponse.Content.ReadAsStreamAsync().ConfigureAwait(false);

            using (var reader = new StreamReader(responseStream))
            {
                var soapDocument = XDocument.Load(reader);

                var processResultElement =
                    soapDocument.Descendants(XName.Get("processResult", SimulatorNamespace.NamespaceName))
                        .FirstOrDefault();

                if (processResultElement == null)
                {
                    throw new FormatException("The simulator response was malformed");
                }

                return Deserialize<SimulatorResponse>(processResultElement.Value);
            }
        }

        private static string Serialize<T>(T objectToSerialize)
        {
            var serializer = new XmlSerializer(typeof(T));

            var settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true
            };

            using (var output = new StringWriter())
            {
                using (var writer = XmlWriter.Create(output, settings))
                {
                    serializer.Serialize(writer, objectToSerialize);
                }
                
                // flush the writer before returning output
                return output.ToString();
            }
        }

        private static T Deserialize<T>(string xml)
        {
            // Have to use XmlSerializer since DataContractSerializer 
            // requires members in a specific order
            var serializer = new XmlSerializer(typeof(T));

            using (var textReader = new StringReader(xml))
            {
                using (var reader = XmlReader.Create(textReader))
                {
                    return (T)serializer.Deserialize(reader);
                }
            }
        }
    }
}
