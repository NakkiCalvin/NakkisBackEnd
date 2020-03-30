using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace BLL.TokenConfiguration
{
    public class TokenConfig
    {
        public const string ISSUER = "MyServer";
        public const string AUDIENCE = "http://http://localhost:3000/";
        const string KEY = "mysupersecret_secretkey!123";   
        public const int LIFETIME = 50; 
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}