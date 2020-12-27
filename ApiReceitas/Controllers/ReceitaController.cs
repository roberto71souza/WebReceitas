using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiReceitas.Models;
using AutoMapper;
using Dominio;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository;

namespace ApiReceitas.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class ReceitaController : ControllerBase
    {

        public IReceitasRepository _receitaApp { get; private set; }
        public IMapper _mapper { get; private set; }

        public ReceitaController(IReceitasRepository receitaApp, IMapper mapper)
        {
            _receitaApp = receitaApp;
            _mapper = mapper;
        }

        // GET: ReceitaController
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReceitaModel>>> Get()
        {
            try
            {
                var result = await _receitaApp.Listar();
                if (result.Count() <= 0)
                {
                    return NoContent();
                }
                var mapResult = _mapper.Map<IEnumerable<ReceitaModel>>(result);
                return Ok(mapResult);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro System: \n {e}");
            }
        }

        // GET: ReceitaController
        [HttpGet("{id}", Name = "GetId")]
        public async Task<ActionResult<ReceitaModel>> GetId(int id)
        {
            try
            {
                var result = await _receitaApp.BuscaID(id);
                if (result == null)
                {
                    return NoContent();
                }
                var mapResult = _mapper.Map<ReceitaModel>(result);
                return Ok(mapResult);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro System: \n {e}");
            }
        }

        //Get : id
        [HttpGet("GetUsuarioReceitas/{id}", Name = "GetUsuarioReceitas")]
        public async Task<ActionResult<IEnumerable<ReceitaModel>>> GetUsuarioReceitas(int id)
        {
            try
            {
                var result = await _receitaApp.ListarReceitasUsuario(id);

                if (result == null)
                {
                    return NotFound();
                }
                var mapResult = _mapper.Map<ReceitaModel[]>(result);
                return Ok(mapResult);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro System: \n {e}");
            }
        }

        // Post: ReceitaController/Create
        [HttpPost]
        public async Task<ActionResult<ReceitaModel>> Adicionar(ReceitaModel modelo)
        {
            try
            {
                var resultMap = _mapper.Map<Receita>(modelo);

                await _receitaApp.Adicionar(resultMap);

                _mapper.Map(resultMap,modelo);

                return Created($"Receita/GetById/{modelo.Id}",modelo);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro System: \n {e}");
            }
        }

        // Put: ReceitaController/Put
        [HttpPut("{id}")]
        public async Task<ActionResult<ReceitaModel>> Editar(int id, ReceitaModel modelo)
        {
            try
            {
                var mapResult = _mapper.Map<Receita>(modelo);

                var resultModel = await _receitaApp.BuscaID(id);

                if (resultModel == null)
                {
                    return NotFound();
                }

                await _receitaApp.Atualizar(mapResult);

                _mapper.Map(modelo,mapResult);

                return Ok(modelo);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro System: \n {e}");
            }
        }

        // Delete: ReceitaController/Delete/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var resultModelo = await _receitaApp.BuscaID(id);

                if (resultModelo == null)
                {
                    return NotFound();
                }
               await _receitaApp.Deletar(resultModelo);

                return Ok();
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro System: \n {e}");
            }
        }
    }
}
