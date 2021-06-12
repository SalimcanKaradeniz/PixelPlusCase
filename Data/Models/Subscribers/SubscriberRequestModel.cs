using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models
{
    public class SubscriberRequestModel
    {
        public string NameSurname { get; set; }
        public string IdentityNumber { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}