using System;
using System.Collections.Generic;
using System.Text;

namespace Hstar.PM.Core.Models
{
    public class BusinessException : Exception
    {
        public BusinessException(string message, int statusCode = 400, Exception ex = null) : base(message, ex)
        {
            this.StatusCode = statusCode;
        }

        public int StatusCode { get; set; }
    }
}
