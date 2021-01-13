using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebAppReceitas.Models;
using WebAppReceitas.Services;

namespace WebAppReceitas.Controllers
{
    public class HomeController : Controller
    {
        public ReceitaService _receitaService { get; set; }
        public IMapper _mapper { get; set; }
        public HomeController(LoginService login, ReceitaService receita, IMapper mapper)
        {
            _mapper = mapper;
            _receitaService = receita;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var result = await _receitaService.ListarReceitas();
                var resultMap = _mapper.Map<ReceitaModel[]>(result);
                return View(resultMap);
            }
            catch (Exception)
            {
                return RedirectToAction("HttpStatusCodeHandler","Home",new{statusCode = this.Response.StatusCode = 400});
            }
        }

        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            switch (statusCode)
            {
                case 400:
                    TempData["errorMessage"] = "Desculpe :( Erro no servidor!!";
                    break;
                case 404:
                    TempData["errorMessage"] = "Desculpe :( pagina nao encontrada!!";
                    break;
            }
            return View("Error");
        }
    }
}