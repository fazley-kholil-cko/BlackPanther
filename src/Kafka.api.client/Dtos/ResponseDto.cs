using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kafka.api.client.Dtos
{
    public class ResponseDto
    {
        public string Id { get; set; }
        public string CardNumber { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string ResponseCode { get; set; }
    }
}
