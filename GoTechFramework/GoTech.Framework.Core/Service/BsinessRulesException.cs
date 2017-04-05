using System;
using System.Collections.Generic;
using System.Text;

namespace GoTech.Framework.Core.Service
{
    public class BsinessRulesException : Exception
    {
        public BsinessRulesException(string msg) : base(msg) { }
    }
}
