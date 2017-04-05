using System;
using System.Collections.Generic;
using System.Text;

namespace GoTech.Framework.Core.Security
{
    public class PasswordHasher 
    {
        public string HashPassword(string password)
        {
            return Crypto.HashPassword(password);
        }

        public bool VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            return Crypto.VerifyHashedPassword(hashedPassword, providedPassword);
        }
    }
}
