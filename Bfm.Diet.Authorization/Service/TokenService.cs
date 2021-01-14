using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Bfm.Diet.Authorization.Model;
using Bfm.Diet.Core;
using Bfm.Diet.Core.Security.Jwt;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Bfm.Diet.Authorization.Service
{
    public class TokenService : ITokenService
    {
        private readonly TokenOptions _tokenOptions;

        public TokenService(IOptions<AppSettings> options)
        {
            _tokenOptions = options.Value.TokenOptions;
        }

        //[WriteLog(Priority = 1)]
        public AccessToken CreateToken(Kullanici user)
        {
            var key = Encoding.ASCII.GetBytes(_tokenOptions.SecurityKey);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _tokenOptions.Issuer,
                Audience = _tokenOptions.Audience,
                Expires = DateTime.Now.AddDays(_tokenOptions.AccessTokenExpiration),
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Sid, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Adi),
                    new Claim(ClaimTypes.Surname, user.Soyadi)
                }),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);
            return new AccessToken
            {
                Token = token,
                Expiration = tokenDescriptor.Expires.Value
            };
        }
    }
}