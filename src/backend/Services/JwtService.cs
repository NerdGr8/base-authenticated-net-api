using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JK.Helpers;
using Microsoft.Extensions.Options;
using JK.DAL.Models;

namespace JK.Services
{
    public class JwtService
    {
        private readonly IOptions<Settings> _settings;
        public JwtService(IOptions<Settings> options)
        {
            _settings = options;
        }
        public string Authenticate(string id, string email, string userType)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            Console.WriteLine(_settings.Value.SecretKey);
            //get key
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_settings.Value.SecretKey));
            //set claims
            var claims = new []{
                    new Claim(JwtRegisteredClaimNames.Jti, id),
                    new Claim(JwtRegisteredClaimNames.Email, email),
                    new Claim(JwtRegisteredClaimNames.Typ, userType)
                };
            //generate token
            var tokenGen = new JwtSecurityToken(
                    _settings.Value.Issuer,
                    _settings.Value.Issuer,
                    claims,
                    expires: DateTime.UtcNow.AddDays(_settings.Value.ExpiresInMinutes),
                    signingCredentials: new SigningCredentials(key, _settings.Value.SecurityAlgorithm)
                );
            //merge and set public token
            var publicToken = new JwtSecurityTokenHandler().WriteToken(tokenGen);
            return publicToken;
        }
    }
}
