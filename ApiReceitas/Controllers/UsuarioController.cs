using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dominio;
using Microsoft.AspNetCore.Mvc;
using Repository.InterfaceUsuario;

namespace ApiReceitas.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class UsuarioController : ControllerBase
    {

        public IUsuarioApp _usuarioApp { get;private set; }

        public UsuarioController(IUsuarioApp myProperty)
        {
            _usuarioApp = myProperty;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Usuario>> Get()
        {
            var result = _usuarioApp.ListarUsuario();
            return Ok(result);
        }

        [HttpPost]
        public ActionResult Post([FromBody]Usuario modelo)
        {
             _usuarioApp.AdicionarUsuario(modelo);
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var result = _usuarioApp.BuscaId(id);
            _usuarioApp.DeleteUsuario(result);
            return Ok();
        }
    }
}
