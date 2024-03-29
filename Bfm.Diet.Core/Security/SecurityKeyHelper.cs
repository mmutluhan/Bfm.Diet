﻿using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Bfm.Diet.Core.Security
{
    public class SecurityKeyHelper
    {
        public static SecurityKey CreateSecurityKey(string securityKey)
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
        }
    }
}