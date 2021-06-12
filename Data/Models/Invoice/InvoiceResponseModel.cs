using Data.DbEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models
{
    public class InvoiceResponseModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int InvoiceNumber { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal? TaxRate { get; set; }
        public bool IsPayment { get; set; }
        public DateTime ExpriyDate { get; set; }
        public DateTime? CreatedAt { get; set; }

        public SubscribersResponseModel Subscribers { get; set; }
    }
}
