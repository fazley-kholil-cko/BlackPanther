using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Checkout.BlackPanther.ApiGW.Data.Model;
using FluentValidation;

namespace Checkout.BlackPanther.ApiGW.Data
{
    public class TransactionRequestValidator : AbstractValidator<TransactionRequest>
    {
        public TransactionRequestValidator()
        {
            RuleFor(t => t.Email).NotEmpty().EmailAddress();
            RuleFor(t => t.Value).NotEmpty();
            RuleFor(t => t.Currency).NotEmpty().Length(3);
            RuleFor(t => t.TrackId).NotEmpty();
            RuleFor(t => t.Name).NotEmpty();
            RuleFor(t => t.Number).NotEmpty().MaximumLength(19);
            RuleFor(t => t.ExpiryMonth).NotEmpty().Length(2);
            RuleFor(t => t.ExpiryYear).NotEmpty().Length(4);
            RuleFor(t => t.Cvv).NotEmpty().Length(3);
        }
    }
}
