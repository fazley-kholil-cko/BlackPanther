using Newtonsoft.Json;
using System;

namespace Request.Validator.Dtos
{
    public class RequestDto
    {
        [JsonProperty(PropertyName = "correlationId")]
        public Guid CorrelationId { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "value")]
        public int Value { get; set; }

        [JsonProperty(PropertyName = "currency")]
        public string Currency { get; set; }

        [JsonProperty(PropertyName = "trackId")]
        public string TrackId { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "number")]
        public string Number { get; set; }

        [JsonProperty(PropertyName = "expiryMonth")]
        public string ExpiryMonth { get; set; }

        [JsonProperty(PropertyName = "expiryYear")]
        public string ExpiryYear { get; set; }

        [JsonProperty(PropertyName = "cvv")]
        public string Cvv { get; set; }
    }
}
