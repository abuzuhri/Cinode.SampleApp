﻿using System;
using System.Collections.Generic;
using System.Text;

namespace GoTech.Framework.Core
{
    public class BsinessRulesException : Exception
    {
        public BsinessRulesException(string msg) : base(msg) { }
    }
}
