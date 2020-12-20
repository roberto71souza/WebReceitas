using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiReceitas.Dtos;
using AutoMapper;
using Dominio;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Repository;
using static ApiReceitas.Startup;

namespace ApiReceitas.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class LoginUsuarioController : ControllerBase
    {

        private readonly ILogger<LoginUsuarioController> _logger;
        private readonly SignInManager<Usuario> _signInManager;
        public UserManager<Usuario> _userManager { get; }
        public IMapper _mapper { get;private set; }

        public LoginUsuarioController(ILogger<LoginUsuarioController> logger, SignInManager<Usuario> signInManager, UserManager<Usuario> userManager, IMapper mapper)
        {
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
            _mapper = mapper;
        }


        [HttpPost]
        public async Task<ActionResult> Login(LoginDto model)
        {
                var user = await _userManager.FindByNameAsync(model.UserName);

                var lockedUser = await _userManager.IsLockedOutAsync(user);

                if (user != null && !lockedUser)
                {
                    var userPass = await _userManager.CheckPasswordAsync(user, model.Password);

                    if (userPass)
                    {
                        var confirmEmail = await _userManager.IsEmailConfirmedAsync(user);

                        if (!confirmEmail)
                        {
                            return Unauthorized("Usuario cadastrado mas nao foi validado!");
                        }
                        await _userManager.ResetAccessFailedCountAsync(user);


                        /* two factory
                        if (await _userManager.GetTwoFactorEnabledAsync(user))
                        {
                            var validator = await _userManager.GetValidTwoFactorProvidersAsync(user);

                            if (validator.Contains("Email"))
                            {
                                var token = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
                                System.IO.File.WriteAllText("email2sv.txt", token);

                                await HttpContext.SignInAsync(IdentityConstants.TwoFactorUserIdScheme,
                                    Store2FA(user.Id, "Email"));

                                return RedirectToAction("TwoFactor");
                            }
                        }
                        var principal = await _userClaimsFactory.CreateAsync(user);*/

                        //Identity.Application nome padrao que o ConfigureApplicationCookie procura
                        await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, null);

                        return RedirectToAction("Logado");
                    }
                    await _userManager.AccessFailedAsync(user);
                    if (await _userManager.IsLockedOutAsync(user))
                    {

                    }
                }
                return Unauthorized("Usuario ou senha errados");
        }

        public IActionResult Logado()
        {
            var lista = new List<string>();
            foreach (var claim in User.Claims)
            {
                lista.Add(claim.Type);
            }
            return Ok(lista);
        }
        [HttpPost]
        public async Task<IActionResult> Registrar(RegistraUsuarioDto model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Email);

                if (user == null)
                {
                    user = new Usuario
                    {
                        UserName = model.Email,
                        Email = model.Email
                    };

                    var result = await _userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        var confirmEmail = Url.Action("ConfirmEmailAddress", "Home",
                            new { token = token, email = user.Email }, Request.Scheme);
                        System.IO.File.WriteAllText("confirmaEmail.txt", confirmEmail);
                        return Ok("Sucesso vejo o email");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            return BadRequest(error.Description);
                        }
                    }
                }
                return Unauthorized("Usuario(email) ja existe no sistema");
            }
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmailAddress(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);

                if (result.Succeeded)
                {
                    return Ok("Usuario authorizado a logar no sistema");
                }
            }
            return BadRequest("Error");
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                    var resetUrl = Url.Action("ResetPassword", "Home",
                        new { Token = token, Email = model.Email }, Request.Scheme);

                    System.IO.File.WriteAllText("resetLink.txt", resetUrl);
                    return Ok("Sucesso Veja o seu email");
                }
                return NotFound("Email nao encontrado");
            }
            return Ok();
        }

        [HttpGet]
        public ActionResult ResetPassword(string token, string email)
        {
            return RedirectToAction("ResetPassword", new ResetPasswordDto { Token = token, Email = email });
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto modelo)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(modelo.Email);

                if (user != null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, modelo.Token, modelo.Password);

                    if (!result.Succeeded)
                    {
                        foreach (var erro in result.Errors)
                        {
                            return BadRequest(erro.Description);
                        }
                    }
                    return Ok("Senha alterada com sucesso");
                }
                return NotFound("Usuario nao encontrado");
            }
            return Ok();
        }
    }
}
