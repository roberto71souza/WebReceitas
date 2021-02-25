using System.Net.Http;
using System.Threading.Tasks;
using WebAppReceitas.Models;
using Newtonsoft.Json;
using System.Text;
using System;
using System.Net;
using WebAppReceitas.Jwt;

namespace WebAppReceitas.Services
{
    public class LoginService
    {
        public HttpClient _client { get; set; }

        public LoginService(IHttpClientFactory factory)
        {
            _client = factory.CreateClient("UrlBase");
        }

        public async Task<object> UsuarioLogin(LoginModel model)
        {
            var usuario = new UsuarioTokenModel();
            StringContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("Login/Login", content);

            string apiResponse = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return apiResponse;
            }

            usuario = UsuarioToken.ConstruirUsuarioToken(apiResponse).Result;

            return usuario;
        }

        public Task<bool> EsqueciSenhaService(EsqueciSenhaModel model, out string token)
        {
            token = string.Empty;

            StringContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var response = _client.PostAsync("Login/ForgotPassword", content).Result;

            if (response.IsSuccessStatusCode)
            {
                var apiResponse = response.Content.ReadAsStringAsync().Result;
                token = apiResponse;

                return Task.FromResult(true);
            }
            else if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                throw new Exception();
            }
            return Task.FromResult(false);
        }

        public async Task<bool> ResetarSenhaService(ResetarSenhaModel model)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("Login/ResetPassword", content);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

    }
}
