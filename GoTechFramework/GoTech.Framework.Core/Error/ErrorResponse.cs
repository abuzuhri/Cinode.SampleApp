using System;
using System.Collections.Generic;
using System.Text;

namespace GoTech.Framework.Core.Error
{
    public class ErrorResponse
    {
        public ErrorResponse()
        {
        }

        public string Message { get; set; }
        public string StackTrace { get; set; }
    }
}
