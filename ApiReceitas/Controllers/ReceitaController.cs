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
    public class ReceitaController : ControllerBase
    {

        public IReceitasRepository _receitaApp { get; private set; }
        public IMapper _mapper { get; private set; }

        public ReceitaController(ServiceResolver receitaApp, IMapper mapper)
        {
            _receitaApp = receitaApp("receita");
            _mapper = mapper;
        }


        // GET: ReceitaController
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReceitaDto>>> Get()
        {
            try
            {
                var result = await _receitaApp.Listar();
                if (result.Count() <= 0)
                {
                    return NoContent();
                }
                var mapResult = _mapper.Map<IEnumerable<ReceitaDto>>(result);
                return Ok(mapResult);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro System: \n {e}");
            }
        }

        //Get : id
        [HttpGet("{id}", Name = "GetId")]
        public async Task<ActionResult> GetId(int id)
        {
            try
            {
                var result = await _receitaApp.BuscaID(id);

                if (result == null)
                {
                    return NotFound();
                }
                var mapResult = _mapper.Map<ReceitaDto>(result);
                return Ok(mapResult);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro System: \n {e}");
            }

        }

        // Post: ReceitaController/Create
        [HttpPost]
        public async Task<ActionResult<ReceitaDto>> Adicionar(ReceitaDto modelo)
        {
            try
            {
                var resultMap = _mapper.Map<Receita>(modelo);

                await _receitaApp.Adicionar(resultMap);

                _mapper.Map(modelo, resultMap);

                return CreatedAtRoute(nameof(GetId), new { Id = modelo.Id }, modelo);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro System: \n {e}");
            }
        }

        // Put: ReceitaController/Put
        [HttpPut("{id}")]
        public async Task<ActionResult<ReceitaDto>> Editar(int id, ReceitaDto modelo)
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

                return NoContent();
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro System: \n {e}");
            }
        }
    }
}
