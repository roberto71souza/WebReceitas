using AutoMapper;
using Dominio;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using WebAppReceitas.Models;
using WebAppReceitas.Services;

namespace WebAppReceitas.Controllers
{
    public class LoginUsuarioController : Controller
    {
        public LoginService _loginService { get; set; }
        public RegistroService _registroService { get; set; }
        public ReceitaService _receitaService { get; set; }
        public IToastNotification _toast { get; }
        public IMapper _mapper { get; set; }
        public LoginUsuarioController(LoginService login, ReceitaService receita, RegistroService registro, IToastNotification toast, IMapper mapper)
        {
            _mapper = mapper;
            _loginService = login;
            _receitaService = receita;
            _registroService = registro;
            _toast = toast;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                var idUser = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                return RedirectToAction("Perfil", "LoginUsuario", new { id = idUser });
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

                    if (usuario is UsuarioModel)
                    {
                        var usuarioModelo = (UsuarioModel)usuario;
                        var claims = new List<Claim>{new Claim(ClaimTypes.Name, usuarioModelo.Nome),
                                                new Claim(ClaimTypes.NameIdentifier, usuarioModelo.Id.ToString())};

                        var claimsIdentity = new ClaimsIdentity(
                            claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity));

                        return RedirectToAction("Perfil", "LoginUsuario", new { id = usuarioModelo.Id });
                    }
                    ModelState.AddModelError("", $"{usuario}");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("HttpStatusCodeHandler", "Home", new { statusCode = this.Response.StatusCode = 500 });
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<ActionResult<Receita>> Perfil(int id)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    var idUser = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    if (id == idUser)
                    {
                        if (ModelState.IsValid)
                        {
                            var receitaUsuario = await _receitaService.ListarReceitasUsuarioId(id);

                            var mapResult = _mapper.Map<ReceitaModel[]>(receitaUsuario);
                            return View(mapResult);
                        }
                        return View();
                    }
                }
                return RedirectToAction("Login");
            }
            catch (Exception)
            {
                return RedirectToAction("HttpStatusCodeHandler", "Home", new { statusCode = this.Response.StatusCode = 500 });
            }
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
                    bool token, unath;
                    var result = await _registroService.RegistrarUsuario(model, out token, out unath);

                    if (token)
                    {
                        var urlResult = Url.Action("ConfirmarEmail", "LoginUsuario",
                            new { token = result, email = model.Email }, Request.Scheme);
                        await Email.Enviar(model, "Confirmacao de email - WebReceitas", urlResult);
                        return RedirectToAction("Confirmacao", new { mensagem = "Usuario cadastrado, agora para validar seu cadastro verifique o email que te enviaremos" });
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
                return RedirectToAction("Confirmacao", new { mensagem = "Usuario validado, agora vc pode logar no sistema!!" });
            }
            else
            {
                _toast.AddErrorToastMessage($"Erro ao validar");
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public IActionResult Partial_Postar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Partial_Postar(ReceitaModel modelo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var idUser = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                    modelo.Data_Publicacao = DateTime.Now.ToString("dd/MM/yyyy");
                    modelo.Usuario = new UsuarioModel { Id = idUser };

                    var result = await _receitaService.PostarReceita(modelo);

                    if (result)
                    {
                        _toast.AddSuccessToastMessage("Receita Postada com sucesso!");
                        return RedirectToAction("Perfil");
                    }
                }

                string messages = string.Join(".<br/>\r\n ", ModelState.Values
                                                        .SelectMany(x => x.Errors)
                                                        .Select(x => x.ErrorMessage));

                _toast.AddErrorToastMessage(String.Join("<br/>\r\n", "Erro ao Postar receita !!!", messages));
                return RedirectToAction("Perfil");
            }
            catch (Exception)
            {
                return RedirectToAction("HttpStatusCodeHandler", "Home", new { statusCode = this.Response.StatusCode = 500 });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Partial_Visualizar(int id)
        {
            try
            {
                var receita = await _receitaService.VisualizarReceitaId(id);
                if (receita == null)
                {
                    return Json("Erro ao carregar receita!!");
                }
                var receitaMap = _mapper.Map<ReceitaModel>(receita);
                return View(receitaMap);
            }
            catch (Exception)
            {
                return Json("Erro no servidor!");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Partial_Editar(int id)
        {
            try
            {
                var receita = await _receitaService.VisualizarReceitaId(id);
                if (receita == null)
                {
                    return Json("Erro ao carregar receita!!");
                }
                var receitaMap = _mapper.Map<ReceitaModel>(receita);
                return View(receitaMap);
            }
            catch (Exception)
            {
                return Json("Erro no servidor!");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Partial_Editar(ReceitaModel modelo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _receitaService.EditarReceita(modelo);

                    if (result)
                    {
                        _toast.AddSuccessToastMessage("Receita atualizada com sucesso!");
                        return RedirectToAction("Perfil");
                    }
                }

                string messages = string.Join(".\n ", ModelState.Values
                                                        .SelectMany(x => x.Errors)
                                                        .Select(x => x.ErrorMessage));

                _toast.AddErrorToastMessage(String.Join("<br/>", "Erro ao atualizar !!!", messages));
                return RedirectToAction("Perfil");
            }
            catch (Exception)
            {
                return RedirectToAction("HttpStatusCodeHandler", "Home", new { statusCode = this.Response.StatusCode = 500 });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Partial_Delete(int id)
        {
            try
            {
                var receita = await _receitaService.VisualizarReceitaId(id);
                if (receita == null)
                {
                    return Json("Erro ao carregar receita!!");
                }
                var receitaMap = _mapper.Map<ReceitaModel>(receita);
                return View(receitaMap);
            }
            catch (Exception)
            {
                return Json("Erro no servidor!"); ;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (id > 0)
                {
                    var result = await _receitaService.DeletarReceita(id);

                    if (result)
                    {
                        _toast.AddSuccessToastMessage("receita deletada com sucesso!");
                        return RedirectToAction("Perfil");
                    }
                }
                _toast.AddErrorToastMessage("Erro ao deletar receita");
                return RedirectToAction("Perfil");
            }
            catch (Exception)
            {
                return RedirectToAction("HttpStatusCodeHandler", "Home", new { statusCode = this.Response.StatusCode = 500 });
            }
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
            TempData["msg"] = $"Tudo certo !! \n{mensagem}";
            return View();
        }
    }
}
