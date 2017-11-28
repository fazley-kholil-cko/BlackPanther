using System;

namespace Kafka.Risk
{
    public class Request
    {
        public Guid CorellationId { get; set; }
        public string Email { get; set; }
        public int Value { get; set; }
        public string Currency { get; set; }
        public string TrackId { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public string ExpiryMonth { get; set; }
        public string ExpiryYear { get; set; }
        public string Cvv { get; set; }
    }
}