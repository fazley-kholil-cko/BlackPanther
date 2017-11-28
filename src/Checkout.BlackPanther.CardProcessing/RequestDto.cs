using System;
using System.Collections.Generic;
using System.Text;

namespace Checkout.BlackPanther.CardProcessing
{
    public class RequestDto
    {
        public string email { get; set; }
        public decimal value { get; set; }
        public string currency { get; set; }
        public string trackId { get; set; }
        public string name { get; set; }
        public string number { get; set; }
        public string expiryMonth { get; set; }
        public string expiryYear { get; set; }
        public string cvv { get; set; }
    }
}
