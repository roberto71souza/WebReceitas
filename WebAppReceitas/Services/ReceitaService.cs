using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Dominio;
using Newtonsoft.Json;

namespace WebAppReceitas.Services
{
    public class ReceitaService
    {
        const string Url = "https://localhost:44311/Receita";

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
    }
}
