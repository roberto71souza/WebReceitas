using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiReceitas.Dtos;
using AutoMapper;
using Dominio;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.InterfaceUsuario;

namespace ApiReceitas.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class UsuarioController : ControllerBase
    {

        public IUsuarioApp _usuarioApp { get;private set; }
        public IMapper _mapper { get;private set; }

        public UsuarioController(IUsuarioApp myProperty, IMapper map)
        {
            _usuarioApp = myProperty;
            _mapper = map;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Usuario>> Get()
        {
            try
            {
                var result = _usuarioApp.ListarUsuario();
                if (result == null)
                {
                    return NoContent();
                }
                var resultMap = _mapper.Map<IEnumerable<UsuarioDto>>(result);
                return Ok(result);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro System: \n {e}");
            }
        }

        [HttpPost]
        public ActionResult Post(Usuario modelo)
        {
            try
            {
                _usuarioApp.AdicionarUsuario(modelo);

                var resultMap = _mapper.Map<UsuarioDto>(modelo);
                return Created($"usuario/{resultMap.Id}", resultMap);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro System: \n {e}");
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var result = _usuarioApp.BuscaId(id);

                if (result == null)
                {
                    return NotFound();
                }
                _usuarioApp.DeleteUsuario(result);
                return Ok();
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro System: \n {e}");
            }
        }
    }
}
