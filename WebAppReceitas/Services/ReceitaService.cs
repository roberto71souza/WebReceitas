using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Dominio;
using Newtonsoft.Json;
using WebAppReceitas.Models;

namespace WebAppReceitas.Services
{
    public class ReceitaService
    {
        const string Url = "https://localhost:44311/Receita";

        public async Task<IEnumerable<Receita>> ListarReceitas()
        {
            var lista = new List<Receita>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{Url}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();

                    lista = JsonConvert.DeserializeObject<List<Receita>>(apiResponse);
                }
            }
            return lista;
        }

        public async Task<IEnumerable<Receita>> ListarReceitasUsuarioId(int id)
        {
            var lista = new List<Receita>();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{Url}/GetUsuarioReceitas/{id}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();

                    lista = JsonConvert.DeserializeObject<List<Receita>>(apiResponse);
                }
            }
            return lista;
        }

        public async Task<bool> PostarReceita(ReceitaModel modelo)
        {
            var receita = new ReceitaModel();
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(modelo), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PostAsync(Url, content))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public async Task<Receita> VisualizarReceitaId(int id)
        {
            var receita = new Receita();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{Url}/{id}"))
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();

                    receita = JsonConvert.DeserializeObject<Receita>(apiResponse);
                }
            }
            return receita;
        }

        public async Task<bool> EditarReceita(ReceitaModel modelo)
        {
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(modelo), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PutAsync($"{Url}/{modelo.Id}", content))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public async Task<bool> DeletarReceita(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync($"{Url}/{id}"))
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
