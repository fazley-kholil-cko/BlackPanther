using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkout.BlackPanther.ApiGW.Data.Model
{
    public class CustomError
    {
        public string Property { get; set; }

        public string ErrorMessage { get; set; }
    }
}
