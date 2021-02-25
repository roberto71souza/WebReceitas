using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebAppReceitas.Models;

namespace WebAppReceitas.Services
{
    public class RegistroService
    {
        public HttpClient _client { get; set; }

        public RegistroService(IHttpClientFactory factory)
        {
            _client = factory.CreateClient("UrlBase");
        }

        public Task<string> RegistrarUsuario(RegistraUsuarioModel model, out bool token, out bool erro)
        {
            token = false;
            erro = false;

            StringContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var response = _client.PostAsync($"Login/Registrar", content).Result;
            var apiResponse = response.Content.ReadAsStringAsync().Result;

            if (response.IsSuccessStatusCode)
            {
                token = true;
                return Task.FromResult(apiResponse);
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                erro = true;
                return Task.FromResult(apiResponse);
            }
            else if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                throw new Exception();
            }
            return (Task<string>)Task.CompletedTask;
        }

        public async Task<bool> VerificaEmail(string token, string email)
        {
            var response = await _client.GetAsync($"Login/ConfirmEmailAddress/?token={token}&email={email}");
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

    }
}