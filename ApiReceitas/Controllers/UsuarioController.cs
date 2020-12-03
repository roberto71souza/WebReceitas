using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiReceitas.Dtos;
using AutoMapper;
using Dominio;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository;
using static ApiReceitas.Startup;

namespace ApiReceitas.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class UsuarioController : ControllerBase
    {

        public IReceitasRepository _usuarioApp { get;private set; }
        public IMapper _mapper { get;private set; }

        public UsuarioController(ServiceResolver myProperty, IMapper map)
        {
            _usuarioApp = myProperty("usuario");
            _mapper = map;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> Get()
        {
            try
            {
                var result = await _usuarioApp.Listar();
                if (result.Count()<=0)
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
        public async Task<ActionResult> Post(Usuario modelo)
        {
            try
            {
               await _usuarioApp.Adicionar(modelo);

                var resultMap = _mapper.Map<UsuarioDto>(modelo);
                return Created($"usuario/{resultMap.Id}", resultMap);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro System: \n {e}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var result = await _usuarioApp.BuscaID(id);

                if (result == null)
                {
                    return NotFound();
                }
               await _usuarioApp.Deletar(result);
                return Ok();
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro System: \n {e}");
            }
        }
    }
}
