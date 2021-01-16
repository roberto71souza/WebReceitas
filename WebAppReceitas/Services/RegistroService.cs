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
        const string Url = "https://localhost:44311/Login";

        public Task<string> RegistrarUsuario(RegistraUsuarioModel model, out bool token, out bool unath)
        {
            token = false;
            unath = false;
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
                using (var response = httpClient.PostAsync($"{Url}/Registrar", content).Result)
                {
                    var apiResponse = response.Content.ReadAsStringAsync().Result;

                    if (response.IsSuccessStatusCode)
                    {
                        token = true;
                        return Task.FromResult(apiResponse);
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        unath = true;
                        return Task.FromResult(apiResponse);
                    }
                }
            }
            return (Task<string>)Task.CompletedTask;
        }

        public async Task<bool> VerificaEmail(string token, string email)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{Url}/ConfirmEmailAddress/?token={token}&email={email}"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

    }
}