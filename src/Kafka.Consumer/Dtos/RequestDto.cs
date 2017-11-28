using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kafka.Consumer.Dtos
{
    public class RequestDto
    {
        public string CardNumber { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
    }
}
