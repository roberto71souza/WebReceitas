using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebAppReceitas.Models;
using WebAppReceitas.Services;
using X.PagedList;

namespace WebAppReceitas.Controllers
{
    public class HomeController : Controller
    {
        public ReceitaService _receitaService { get; set; }
        public IMapper _mapper { get; set; }
        public HomeController(ReceitaService receita, IMapper mapper)
        {
            _mapper = mapper;
            _receitaService = receita;
        }

        public async Task<IActionResult> Index(int? page)
        {
            try
            {
                var pageNumber = page ?? 1;
                var pageSize = 3;
                var result = await _receitaService.ListarReceitas();
                var resultMap = _mapper.Map<IEnumerable<ReceitaModel>>(result);

                var resultPage = resultMap.ToPagedList(pageNumber, pageSize);
                return View(resultPage);
            }
            catch (Exception)
            {
                return RedirectToAction("HttpStatusCodeHandler", "Home", new { statusCode = this.Response.StatusCode = 500 });
            }
        }

        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            switch (statusCode)
            {
                case 404:
                    TempData["errorMessage"] = "Desculpe :( pagina nao encontrada!!";
                    break;
                case 500:
                    TempData["errorMessage"] = "Desculpe :( Erro no servidor!!";
                    break;
                default:
                    TempData["errorMessage"] = "Error default!!";
                    break;
            }
            return View("Error");
        }
    }
}