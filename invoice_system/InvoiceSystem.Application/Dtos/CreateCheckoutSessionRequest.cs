using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceSystem.Application.Dtos
{
    public class CreateCheckoutSessionRequest
    {
        public int InvoiceId { get; set; }
    }
}
