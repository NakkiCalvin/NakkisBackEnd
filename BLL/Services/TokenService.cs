using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BLL.Managers;
using BLL.TokenConfiguration;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BLL.Services
{
    public class TokenService : ITokenService
    {
        public TokenService(IConfiguration configuration)
        {
        }

        public string GetEncodedJwtToken(string userEmail)
        {
            var claims = new List<Claim> { new Claim(JwtRegisteredClaimNames.Sub, userEmail) };


            var jwtToken = new JwtSecurityToken(
                TokenConfig.ISSUER,
                TokenConfig.AUDIENCE,
                claims,
                expires: DateTime.Now.Add(TimeSpan.FromMinutes(TokenConfig.LIFETIME)),
                signingCredentials: new SigningCredentials(TokenConfig.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
            );
            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }
    }
}
