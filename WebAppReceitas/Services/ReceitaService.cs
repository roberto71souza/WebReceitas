using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Dominio;
using Newtonsoft.Json;
using WebAppReceitas.Models;

namespace WebAppReceitas.Services
{
    public class ReceitaService
    {
        public HttpClient _client { get; set; }

        public ReceitaService(IHttpClientFactory factory)
        {
            _client = factory.CreateClient("UrlBase");
        }

        public async Task<IEnumerable<Receita>> ListarReceitas()
        {
            var lista = new List<Receita>();

            var response = await _client.GetAsync($"Receita");

            string apiResponse = await response.Content.ReadAsStringAsync();

            lista = JsonConvert.DeserializeObject<List<Receita>>(apiResponse);
            return lista;
        }

        public async Task<IEnumerable<Receita>> ListarReceitasUsuarioId(int id, string token)
        {
            var lista = new List<Receita>();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{token}");

            var response = await _client.GetAsync($"Receita/GetUsuarioReceitas/{id}");

            string apiResponse = await response.Content.ReadAsStringAsync();
            lista = JsonConvert.DeserializeObject<List<Receita>>(apiResponse);

            return lista;
        }

        public async Task<bool> PostarReceita(ReceitaModel modelo, string token)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(modelo), Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{token}");

            var response = await _client.PostAsync("Receita", content);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
        public async Task<Receita> VisualizarReceitaId(int id, string token)
        {
            var receita = new Receita();

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{token}");

            var response = await _client.GetAsync($"Receita/{id}");
            var apiResponse = await response.Content.ReadAsStringAsync();

            receita = JsonConvert.DeserializeObject<Receita>(apiResponse);
            return receita;
        }

        public async Task<bool> EditarReceita(ReceitaModel modelo, string token)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(modelo), Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{token}");

            var response = await _client.PutAsync($"Receita/{modelo.Id}", content);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> DeletarReceita(int id, string token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{token}");
            var response = await _client.DeleteAsync($"Receita/{id}");

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
    }
}
