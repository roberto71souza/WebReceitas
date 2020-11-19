using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dominio;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.InterfaceReceita;

namespace ApiReceitas.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class ReceitaController : ControllerBase
    {

        public IReceitasApp _receitaApp { get; private set; }

        public ReceitaController(IReceitasApp receitaApp)
        {
            _receitaApp = receitaApp;
        }

        // GET: ReceitaController
        [HttpGet]
        public ActionResult<IEnumerable<Receita>> Get()
        {
            var result = _receitaApp.ListarReceitas();
            return Ok(result);
        }

        //Get : id
        [HttpGet("{id}")]
        public ActionResult GetId(int id)
        {
            var result = _receitaApp.BuscaID(id);

            return Ok(result);
        }

        // Post: ReceitaController/Create
        [HttpPost]
        public ActionResult Adicionar(Receita modelo)
        {
            _receitaApp.AdicionarReceita(modelo);

            return Ok();
        }

        [HttpPut("{id}")]
        public ActionResult Editar(int id, Receita modelo)
        {
            var resultModel = _receitaApp.BuscaID(id);
            _receitaApp.Atualizar(modelo);
            return Ok();
        }

        // GET: ReceitaController/Delete/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var resultModelo = _receitaApp.BuscaID(id);
            _receitaApp.DeletarReceita(resultModelo);

            return NoContent();
        }
    }
}
