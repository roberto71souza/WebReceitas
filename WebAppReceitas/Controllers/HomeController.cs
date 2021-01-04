using AutoMapper;
using Dominio;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAppReceitas.Models;
using WebAppReceitas.Services;

namespace WebAppReceitas.Controllers
{
    public class HomeController : Controller
    {
        public LoginService _loginService { get; set; }
        public ReceitaService _receitaService { get; set; }
        public IMapper _mapper { get; set; }
        public HomeController(LoginService login, ReceitaService receita, IMapper _mapper)
        {
            this._mapper = _mapper;
            _loginService = login;
            _receitaService = receita;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                var idUser = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                return RedirectToAction("Perfil", "Home", new { id = idUser });
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            try
            {
                var usuario = new object();

                if (ModelState.IsValid)
                {
                    usuario = await _loginService.UsuarioLogin(model);

                    if (usuario is Usuario)
                    {
                        var usuarioModelo = (Usuario)usuario;
                        var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuarioModelo.Nome),
                        new Claim(ClaimTypes.NameIdentifier, usuarioModelo.Id.ToString()),
                        new Claim("FullName", usuarioModelo.Nome),
                    };

                        var claimsIdentity = new ClaimsIdentity(
                            claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity));

                        return RedirectToAction("Perfil", "Home", new { id = usuarioModelo.Id });
                    }
                    ModelState.AddModelError("", $"{usuario}");
                }
            }
            catch (Exception e)
            {
                return this.StatusCode(400, $"Erro :{e}");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index");
        }

        public IActionResult Registrar()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult<Receita>> Perfil(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    var receitaUsuario = await _receitaService.ListarReceitasUsuarioId(id);

                    var mapResult = _mapper.Map<ReceitaModel[]>(receitaUsuario);
                    return View(mapResult);
                }
                return View();
            }
            return RedirectToAction("Login");
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
