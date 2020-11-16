using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dominio;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiReceitas.Controllers
{
    [ApiController]
    [Route("Api")]
    public class ReceitaController : ControllerBase
    {
        // GET: ReceitaController
        [HttpGet]
        public ActionResult<IEnumerable<Usuario>> Get()
        {
            var result = new List<Usuario>();
            result.Add(new Usuario { Id = 1, Cidade = "Sao paulo",Data_Nascimento= DateTime.Parse("23/08/1996"), Nome = "Roberto", Receita = new List<Receita> { { new Receita { Id = 1, Titulo = "a Melhor" } } } });
            return Ok(result);
        }

        //Get : id
        [HttpGet("{id}")]
        public ActionResult GetId(int id)
        {
            var result = $"Get retorna id {id}";

            return Ok(result);
        }

        // Post: ReceitaController/Create
        [HttpPost]
        public ActionResult Adicionar(Receita modelo)
        {
            return Ok();
        }
        
        [HttpPut("{id}")]
        public ActionResult Editar(int id, Receita modelo)
        {
            return Ok();
        }

        // GET: ReceitaController/Delete/5
        public ActionResult Delete(int id)
        {
            return Ok();
        }
    }
}
