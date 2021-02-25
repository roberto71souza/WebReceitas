using Dominio;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ApiReceitas.Jwt
{
    public class JwtMetodos
    {
        public IConfiguration _config { get; set; }

        public JwtMetodos(IConfiguration configuration)
        {
            _config = configuration;
        }

        public Task<string> GenerateJwtToken(Usuario usuario)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,usuario.Nome),
                new Claim(ClaimTypes.NameIdentifier,usuario.Id.ToString())
            };

            var key = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(_config.GetSection("AppSettings:Token").Value));

            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDesc = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(4),
                SigningCredentials = credential
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDesc);
            var result = tokenHandler.WriteToken(token);
            return Task.FromResult(result);
        }
    }
}
