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
            var usuario = new UsuarioModel();

            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PostAsync($"{Url}/Login", content))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        usuario = JsonConvert.DeserializeObject<UsuarioModel>(apiResponse);
                        return usuario;
                    }
                    else if (response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        throw new Exception();
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
