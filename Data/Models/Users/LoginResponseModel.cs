using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models
{
    public class LoginResponseModel
    {
        public string Token { get; set; }
        public DateTime TokenExpireDate { get; set; }
    }
}
