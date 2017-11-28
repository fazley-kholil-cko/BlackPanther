using System;
using System.Collections.Generic;

namespace Checkout.BlackPanther.CardProcessing.Model
{
    public class AcquirerConfiguration
    {
        public AcquirerConfiguration(
            int id,
            string name,
            string uri,
            string terminalId,
            string password,
            string token,
            string sender,
            string merchantId,
            Dictionary<string, string> processorSettings)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrEmpty(uri)) throw new ArgumentNullException(nameof(uri));

            Id = id;
            Name = name;
            Uri = uri;
            TerminalId = terminalId;
            Password = password;
            Token = token;
            Sender = sender;
            MerchantId = merchantId;
            ProcessorSettings = processorSettings;
        }

        /// <summary>
        /// Gets the unique identifier of the acquirer.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Gets the name of the acquirer.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the acquirer service URI.
        /// </summary>
        public string Uri { get; }

        /// <summary>
        /// Gets the terminal identifier used to authenticate with the acquirer.
        /// </summary>
        public string TerminalId { get; }

        /// <summary>
        /// Gets the password used to authetnicate with the acquirer.
        /// </summary>
        public string Password { get; }

        /// <summary>
        /// Gets the acquirer processor token - Ask Wachill
        /// </summary>
        public string Token { get; }

        /// <summary>
        /// May need more than this e.g. Sender Name/Identifier/Short-ref from Acquirer table - Ask Wachill
        /// </summary>
        public string Sender { get; }

        /// <summary>
        /// The acquirer's merchant identifier
        /// </summary>
        public string MerchantId { get; }

        /// <summary>
        /// Processor specific settings set within the hub
        /// </summary>
        public Dictionary<string, string> ProcessorSettings { get; set; }
    }
}