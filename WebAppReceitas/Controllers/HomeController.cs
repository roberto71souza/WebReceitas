using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebAppReceitas.Models;

namespace WebAppReceitas.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Perfil()
        {
            return View();
        }
        public IActionResult Partial_Postar()
        {
            return View();
        }
        public IActionResult Partial_Visualizar()
        {
            var receita = new ReceitaModel { Id = 1, Titulo = "Camarao fresco", Conteudo = "1- kilo de camarao,1-cebola, manjericao", Acessório = "uma pa", Data_Publicacao = DateTime.Now };
            return View(receita);
        }
        public IActionResult Partial_Editar()
        {
            return View();
        }
        public IActionResult Partial_Delete()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Registrar()
        {
            return View();
        }

        public IActionResult EsqueciSenha()
        {
            return View();
        }

        public IActionResult ResetarSenha(/*string token,string email*/)
        {
            return View();
        }

        public IActionResult Confirmacao(string mensagem)
        {
            TempData["msg"] = $"Tudo certo !! {mensagem}";
            return View();
        }

    }
}
