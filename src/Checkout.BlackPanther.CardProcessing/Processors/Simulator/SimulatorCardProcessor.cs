using Checkout.BlackPanther.CardProcessing.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;


namespace Checkout.BlackPanther.CardProcessing.Processors.Simulator
{
    public class SimulatorCardProcessor
    {
        private readonly HttpClient _httpClient;

        public SimulatorCardProcessor(HttpClient httpClient)
        {
            if (httpClient == null) throw new ArgumentNullException(nameof(httpClient));
            _httpClient = httpClient;
        }

        public async Task<CardProcessorResponse> ProcessAsync(CardProcessorRequest cardProcessorRequest)
        {
            if (cardProcessorRequest == null) throw new ArgumentNullException(nameof(cardProcessorRequest));

            var simulatorRequest = CreateSimulatorRequest(cardProcessorRequest);

            var simulatorClientOptions = new SoapClientOptions
            {
                Uri = cardProcessorRequest.AcquirerConfiguration.Uri
            };

            var simulatorClient = new SoapClient(simulatorClientOptions, _httpClient);

            //using (var op = _logger.BeginOperation("Sending request to Checkout Simulator"))
            //{
            var simulatorResponse = await simulatorClient.ProcessAsync(simulatorRequest);
            //op.Complete();

            return CreateCardProcessorResponse(simulatorResponse);
            //}
        }



        private SimulatorRequest CreateSimulatorRequest(CardProcessorRequest cardProcessorRequest)
        {
            var acquirer = cardProcessorRequest.AcquirerConfiguration;
            var chargeRequest = cardProcessorRequest.ChargeRequest;

            var simulatorRequest = new SimulatorRequest();

            simulatorRequest.ProcessId = acquirer.TerminalId;
            simulatorRequest.ProcessPassword = acquirer.Password;
            simulatorRequest.ServiceUrl = acquirer.Uri;
            simulatorRequest.Token = acquirer.Token;
            simulatorRequest.Sender = acquirer.Sender;
            simulatorRequest.ActionCode = GetAcquirerActionCode(chargeRequest.ActionCode);
            simulatorRequest.Amount = CalculateAmount(chargeRequest);
            simulatorRequest.Currency = chargeRequest.Currency.Iso3Code;
            simulatorRequest.CurrencyCode = chargeRequest.Currency.NumericCode;
            simulatorRequest.CurrencyExponent = chargeRequest.Currency.Exponent;
            simulatorRequest.MerchantId = chargeRequest.Merchant.Id.ToString();
            simulatorRequest.Mcc = chargeRequest.Merchant.CategoryCode;
            simulatorRequest.AcceptorName = chargeRequest.BillingDescriptor;
            simulatorRequest.AcceptorCity = chargeRequest.BillingDescriptorCity;
            simulatorRequest.AcceptorCountry = chargeRequest.Merchant.CountryIso2Code;
            // Not sure why we do this; and pass billing descriptor in Accept Name:
            simulatorRequest.BillingDescriptor = chargeRequest.IsDynamicDescriptor ? string.Empty : "null";
            simulatorRequest.PosCountryCode = chargeRequest.Merchant.CountryIso2Code;
            simulatorRequest.Channel = "1"; // TransactionType.Regular from old Gateway
            simulatorRequest.SubEntityAction = (int)chargeRequest.ActionCode;
            simulatorRequest.TrackId = cardProcessorRequest.InternalTransactionId.ToString();
            simulatorRequest.TransactionId = cardProcessorRequest.InternalTransactionId.ToString();
            simulatorRequest.OriginalTransactionId = chargeRequest.ChargeId;
            simulatorRequest.ParentTransactionId = chargeRequest.ChargeId;
            simulatorRequest.SubEntityOriginalTransactionId = chargeRequest.AcquirerTransactionId;
            // 3DS
            simulatorRequest.Eci = chargeRequest.Eci;
            simulatorRequest.Cavv = chargeRequest.Cavv;
            simulatorRequest.Xid = chargeRequest.Xid;
            simulatorRequest.ParesStatus = chargeRequest.ParesStatus;


            if (chargeRequest.Card != null)
            {
                simulatorRequest.Member = chargeRequest.Card.CardholderName;
                simulatorRequest.CardNumber = chargeRequest.Card.Number;
                simulatorRequest.ExpiryMonth = chargeRequest.Card.ExpiryMonth.ToString().PadLeft(2, '0');
                simulatorRequest.ExpiryYear = chargeRequest.Card.ExpiryYear.ToString();
                simulatorRequest.Cvv2 = chargeRequest.Card.Cvv;
                simulatorRequest.CardType = chargeRequest.Card.Scheme.ToString("G");


                //if (processCardRequest.Card.BillingAddress != null)
                //{
                //    var address = processCardRequest.Card.BillingAddress;
                //    simulatorRequest.BillingAddress = address.Line1;
                //    simulatorRequest.BillingAddress2 = address.Line2;
                //    simulatorRequest.BillingCity = address.TownCity;
                //    simulatorRequest.BillingState = address.State;
                //    simulatorRequest.BillingCountryIso2Code = address.Country.Iso2Code;
                //    simulatorRequest.BillingCountryIso3Code = address.Country.Iso3Code;
                //    simulatorRequest.ZipCode = address.ZipCode;
                //}
            }
            else
            {
                simulatorRequest.CardNumber = string.Empty;
            }

            if (chargeRequest.Metadata != null)
            {
                simulatorRequest.Udf1 = GetValueFromMetadata("udf1", chargeRequest.Metadata);
                simulatorRequest.Udf2 = GetValueFromMetadata("udf2", chargeRequest.Metadata);
                simulatorRequest.Udf3 = GetValueFromMetadata("udf3", chargeRequest.Metadata);
                simulatorRequest.Udf4 = GetValueFromMetadata("udf4", chargeRequest.Metadata);
                simulatorRequest.Udf5 = GetValueFromMetadata("udf5", chargeRequest.Metadata);
            }

            return simulatorRequest;
        }

        private int GetAcquirerActionCode(GatewayActionCode actionCode)
        {
            /*
                Supported by simulator:
            
                AcquirerActionCode	GatewayActionCode	Description
                1	                1	                Purchase
                2	                2	                Refund
                3	                3	                Void Purchase
                4	                4	                Authorisation
                5	                5	                Capture
                7	                7	                Void Capture
                9	                9	                Void Authorisation
                20	                20	                Account Verfication
            */


            switch (actionCode)
            {
                case GatewayActionCode.Payout: // HACK
                case GatewayActionCode.Authorise:
                    return 4;
                case GatewayActionCode.Capture:
                    return 5;
                case GatewayActionCode.Refund:
                    return 2;
                case GatewayActionCode.VoidAuthorization:
                    return 9;
            }

            throw new ArgumentException($"Gateway Action Code '{actionCode}' is not supported by the Simulator.");
        }

        private string CalculateAmount(ChargeRequest chargeRequest)
        {
            var format = chargeRequest.Currency.Exponent == 3 ? "#0.000" : "#0.00";
            return chargeRequest.Amount.ToString(format);
        }

        private CardProcessorResponse CreateCardProcessorResponse(SimulatorResponse simulatorResponse)
        {
            return new CardProcessorResponse
            {
                AcquirerTransactionId = simulatorResponse.TransactionId,
                AcquirerReferenceNumber = simulatorResponse.ReferenceId,
                AcquirerResponse = simulatorResponse.SubEntityResponse,
                AcquirerResponseCode = string.IsNullOrEmpty(simulatorResponse.ResponseCode) ? simulatorResponse.ErrorCode : simulatorResponse.ResponseCode,
                AuthCode = simulatorResponse.AuthCode,
                AvsCheckResult = simulatorResponse.AvsCheck,
                CvvCheckResult = simulatorResponse.Cvv2
            };
        }

        private string GetValueFromMetadata(string key, Dictionary<string, string> metadata)
        {
            string udfValue = null;
            metadata.TryGetValue(key, out udfValue);

            return udfValue;
        }
    }
}
