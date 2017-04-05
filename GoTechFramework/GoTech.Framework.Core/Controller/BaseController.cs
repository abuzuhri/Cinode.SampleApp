using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace GoTech.Framework.Core.Controller
{
    public class BaseController : Microsoft.AspNetCore.Mvc.Controller
    {
       
        public override SignInResult SignIn(ClaimsPrincipal principal, string authenticationScheme)
        {
            
            return base.SignIn(principal, authenticationScheme);
        }
    }
}
