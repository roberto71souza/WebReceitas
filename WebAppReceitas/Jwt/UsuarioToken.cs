using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using WebAppReceitas.Models;

namespace WebAppReceitas.Jwt
{
    public class UsuarioToken
    {
        public static Task<UsuarioTokenModel> ConstruirUsuarioToken(string token)
        {
            UsuarioTokenModel usuario = new UsuarioTokenModel();
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
            usuario.Username = jsonToken.Claims.First(claim => claim.Type == "unique_name").Value;
            usuario.Id = int.Parse(jsonToken.Claims.First(claim => claim.Type == "nameid").Value);
            usuario.Token = token;

            return Task.FromResult(usuario);
        }

        public static bool TokenExpirado(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
            var data = int.Parse(jsonToken.Claims.First(claim => claim.Type == "exp").Value);

            var dataExp = DateTimeOffset.FromUnixTimeSeconds(data).DateTime.ToLocalTime();
            var dateNow = DateTime.UtcNow.ToLocalTime();

            if (dataExp < dateNow)
            {
                return true;
            }
            return false;
        }

    }
}