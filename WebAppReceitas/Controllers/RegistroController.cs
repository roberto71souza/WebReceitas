using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebAppReceitas.Models;
using WebAppReceitas.Services;

namespace WebAppReceitas.Controllers
{
    public class RegistroController : Controller
    {

        public RegistroService _registroService { get; set; }
        public IToastNotification _toast { get; }
        public RegistroController(RegistroService registro, IToastNotification toast)
        {
            _registroService = registro;
            _toast = toast;
        }

        [HttpGet]
        public IActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registrar(RegistraUsuarioModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool token, erro;
                    var result = await _registroService.RegistrarUsuario(model, out token, out erro);

                    if (token)
                    {
                        string msg = "Usuario cadastrado, agora para validar seu cadastro verifique o email que te enviaremos";
                        var urlResult = Url.Action("ConfirmarEmail", "Registro",
                            new { token = result, email = model.Email }, Request.Scheme);
                        await Email.Enviar(model, "Confirmacao de email - WebReceitas", urlResult);

                        return RedirectToAction("Confirmacao", "LoginUsuario", new { mensagem = msg });
                    }
                    else
                    {
                        _toast.AddErrorToastMessage($"{result}");
                        return View();
                    }
                }
                string messages = string.Join(".<br/>\r\n ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));

                _toast.AddErrorToastMessage(String.Join("<br/>\r\n", "Erro ao cadastrar Usuario !!!", messages));

                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("HttpStatusCodeHandler", "Home", new { statusCode = this.Response.StatusCode = 500 });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmarEmail(string token, string email)
        {
            var result = await _registroService.VerificaEmail(token, email);
            if (result)
            {
                return RedirectToAction("Confirmacao", "LoginUsuario", new { mensagem = "Usuario validado, agora vc pode logar no sistema!!" });
            }
            else
            {
                _toast.AddErrorToastMessage($"Erro ao validar");
                return RedirectToAction("Index", "Home");
            }
        }

    }
}
