using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models
{
    public class SubscribersResponseModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int SubscriberNumber { get; set; }
        public string NameSurname { get; set; }
        public string IdentityNumber { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public bool IsCancelled { get; set; }
        public bool IsRefund { get; set; }
        public decimal DepositFee { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
