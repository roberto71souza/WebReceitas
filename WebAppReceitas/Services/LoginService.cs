using Dominio;
using System.Net.Http;
using System.Threading.Tasks;
using WebAppReceitas.Models;
using Newtonsoft.Json;
using System.Text;
using System;
using System.Net;

namespace WebAppReceitas.Services
{
    public class LoginService
    {
        const string Url = "https://localhost:44311/Login";

        public async Task<object> UsuarioLogin(LoginModel model)
        {
            var usuario = new Usuario();

            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PostAsync($"{Url}/Login", content))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        usuario = JsonConvert.DeserializeObject<Usuario>(apiResponse);
                        return usuario;

                    }
                    else if (response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        throw new Exception(apiResponse);
                    }
                    else
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();

                        return apiResponse;
                    }
                }
            }
        }
    }
}
