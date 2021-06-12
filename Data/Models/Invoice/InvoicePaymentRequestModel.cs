using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models
{
    public class InvoicePaymentRequestModel
    {
        public int InvoiceNumber { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
