using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiReceitas.Dtos;
using AutoMapper;
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
        public IMapper _mapper { get; private set; }

        public ReceitaController(IReceitasApp receitaApp, IMapper mapper)
        {
            _receitaApp = receitaApp;
            _mapper = mapper;
        }

        // GET: ReceitaController
        [HttpGet]
        public ActionResult<IEnumerable<Receita>> Get()
        {
            try
            {
                var result = _receitaApp.ListarReceitas();
                var mapResult = _mapper.Map<IEnumerable<ReceitaDto>>(result);
                return Ok(mapResult);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro System: \n {e}");
            }
        }

        //Get : id
        [HttpGet("{id}")]
        public ActionResult GetId(int id)
        {
            try
            {
                var result = _receitaApp.BuscaID(id);

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
        public ActionResult<ReceitaDto> Adicionar(Receita modelo)
        {
            try
            {
                _receitaApp.AdicionarReceita(modelo);

                var resultMap = _mapper.Map<ReceitaDto>(modelo);

                return Created($"Receitas/{resultMap.Id}", resultMap);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro System: \n {e}");
            }
        }

        // Put: ReceitaController/Put
        [HttpPut("{id}")]
        public ActionResult<ReceitaDto> Editar(int id, Receita modelo)
        {
            try
            {
                var resultModel = _receitaApp.BuscaID(id);

                if (resultModel == null)
                {
                    return NotFound();
                }
                _receitaApp.Atualizar(modelo);

                var mapResult = _mapper.Map<ReceitaDto>(resultModel);

                return Ok(mapResult);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro System: \n {e}");
            }
        }

        // Delete: ReceitaController/Delete/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var resultModelo = _receitaApp.BuscaID(id);

                if (resultModelo == null)
                {
                    return NotFound();
                }
                _receitaApp.DeletarReceita(resultModelo);

                return NoContent();
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro System: \n {e}");
            }
        }
    }
}
