using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models
{
    public class ReturnModel<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public object Errors { get; set; }
        public T Data { get; set; }
    }
}
