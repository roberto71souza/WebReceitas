using Dominio;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAppReceitas.Models;
using WebAppReceitas.Services;

namespace WebAppReceitas.Controllers
{
    public class LoginUsuarioController : Controller
    {
        public LoginService _loginService { get; set; }
        public IToastNotification _toast { get; }
        public LoginUsuarioController(LoginService login, IToastNotification toast)
        {
            _loginService = login;
            _toast = toast;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                var idUser = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                return RedirectToAction("Perfil", "Perfil", new { id = idUser });
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
                        var claims = new List<Claim>{new Claim(ClaimTypes.Name, usuarioModelo.Nome),
                                                new Claim(ClaimTypes.NameIdentifier, usuarioModelo.Id.ToString())};

                        var claimsIdentity = new ClaimsIdentity(
                            claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity));

                        return RedirectToAction("Perfil", "Perfil", new { id = usuarioModelo.Id });
                    }
                    ModelState.AddModelError("", $"{usuario}");
                }
                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("HttpStatusCodeHandler", "Home", new { statusCode = this.Response.StatusCode = 500 });
            }
        }

        [HttpGet]
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public IActionResult EsqueciSenha()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EsqueciSenha(EsqueciSenhaModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string token;

                    var result = await _loginService.EsqueciSenhaService(model, out token);
                    if (result)
                    {
                        var urlResult = Url.Action("ResetarSenha", "LoginUsuario",
                            new { token = token, email = model.Email }, Request.Scheme);
                        var emailModel = new RegistraUsuarioModel
                        {
                            Email = model.Email,
                        };
                        await Email.Enviar(emailModel, "Alterar senha - WebReceitas", urlResult);

                        var msg = "Agora verifique seu email e siga as istrucoes enviadas";
                        return RedirectToAction("Confirmacao", "LoginUsuario", new { mensagem = msg });
                    }
                    ModelState.AddModelError("", "Este usuario(email) nao existe!!");
                    return View();
                }
                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("HttpStatusCodeHandler", "Home", new { statusCode = this.Response.StatusCode = 500 });
            }
        }

        [HttpGet]
        public IActionResult ResetarSenha(string token, string email)
        {
            var model = new ResetarSenhaModel { Token = token, Email = email };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetarSenha(ResetarSenhaModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _loginService.ResetarSenhaService(model);

                    if (result)
                    {
                        _toast.AddSuccessToastMessage("Senha alterada com sucesso !!!");
                        return View("Login");
                    }
                }
                _toast.AddErrorToastMessage("Erro ao atualizar senha !!!");
                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {
                return RedirectToAction("HttpStatusCodeHandler", "Home", new { statusCode = this.Response.StatusCode = 500 });
            }
        }

        public IActionResult Confirmacao(string mensagem)
        {
            TempData["msg"] = $"Tudo certo !! \n{mensagem}";
            return View();
        }
    }
}
