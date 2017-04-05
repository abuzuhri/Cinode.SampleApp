using System;
using System.Collections.Generic;
using System.Text;

namespace GoTech.Framework.Core.Configration
{
    public class GoTech
    {
        public bool BusinessRuleValidationIsEnabled { get; set; }
        public string BusinessRuleValidationAssemblyname { get; set; }
        public string BusinessRuleValidationNamespace { get; set; }
        public string ConnectionString { get; set; }
        public string APISecurityKey { get; set; }
        public int SecurityTokenExpirationDay { get; set; }
        public string SecurityTokenIssuer { get; set; }
        public string SecurityTokenAudience { get; set; }
    }
}
