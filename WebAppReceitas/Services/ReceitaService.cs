using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Dominio;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebAppReceitas.Models;

namespace WebAppReceitas.Services
{
    public class ReceitaService
    {
        public IHttpClientFactory _factory { get; set; }
        public HttpClient _client { get => _factory.CreateClient("UrlBase"); }

        public ReceitaService(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        public async Task<IEnumerable<Receita>> ListarReceitas()
        {
            var lista = new List<Receita>();
            var response = await _client.GetAsync($"Receita");

            string apiResponse = await response.Content.ReadAsStringAsync();

            lista = JsonConvert.DeserializeObject<List<Receita>>(apiResponse);
            return lista;
        }

        public async Task<IEnumerable<Receita>> ListarReceitasUsuarioId(int id)
        {
            var lista = new List<Receita>();
            var response = await _client.GetAsync($"Receita/GetUsuarioReceitas/{id}");

            string apiResponse = await response.Content.ReadAsStringAsync();
            lista = JsonConvert.DeserializeObject<List<Receita>>(apiResponse);

            return lista;
        }

        public async Task<bool> PostarReceita(ReceitaModel modelo)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(modelo), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("Receita", content);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
        public async Task<Receita> VisualizarReceitaId(int id)
        {
            var receita = new Receita();

            var response = await _client.GetAsync($"Receita/{id}");
            var apiResponse = await response.Content.ReadAsStringAsync();

            receita = JsonConvert.DeserializeObject<Receita>(apiResponse);
            return receita;
        }

        public async Task<bool> EditarReceita(ReceitaModel modelo)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(modelo), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"Receita/{modelo.Id}", content);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> DeletarReceita(int id)
        {
            var response = await _client.DeleteAsync($"Receita/{id}");

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
    }
}
