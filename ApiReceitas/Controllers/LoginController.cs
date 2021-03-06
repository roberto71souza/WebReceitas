﻿using ApiReceitas.Jwt;
using ApiReceitas.Models;
using Dominio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiReceitas.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    [AllowAnonymous]
    public class LoginController : ControllerBase
    {
        private SignInManager<Usuario> _signIn { get; }

        public JwtMetodos _jwtMetodos { get; set; }

        private UserManager<Usuario> _userManager { get; }

        public LoginController(SignInManager<Usuario> signInManager, UserManager<Usuario> userManager, JwtMetodos jwt)
        {
            _jwtMetodos = jwt;
            _signIn = signInManager;
            _userManager = userManager;
        }


        [HttpPost("Login")]
        public async Task<ActionResult> Login(LoginModel model)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(model.UserName);

                if (user != null)
                {
                    var confirmEmail = await _userManager.IsEmailConfirmedAsync(user);

                    if (!confirmEmail)
                    {
                        return Unauthorized("Usuario cadastrado mas nao foi validado!");
                    }
                    var lockedUser = await _userManager.IsLockedOutAsync(user);

                    if (!lockedUser)
                    {
                        var userPass = await _signIn.CheckPasswordSignInAsync(user, model.Password, true);

                        if (userPass.Succeeded)
                        {
                            await _userManager.ResetAccessFailedCountAsync(user);
                            var jwtToken = _jwtMetodos.GenerateJwtToken(user).Result;

                            return Ok(jwtToken);
                        }
                        else
                        {
                            await _userManager.AccessFailedAsync(user);
                            return Unauthorized("Senha errada !!!");
                        }
                    }
                    return Unauthorized("Usuario bloqueado!Aguarde o desbloqueio ou contate-nos");
                }
                return Unauthorized("Usuario nao existe");
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error API:\n {e},\n Codigo:{this.Response.StatusCode}");
            }
        }


        [HttpPost("Registrar")]
        public async Task<IActionResult> Registrar(RegistraUsuarioModel model)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(model.Email);

                if (user == null)
                {
                    user = new Usuario
                    {
                        Nome = model.Nome,
                        Email = model.Email,
                        UserName = model.Email,
                        Cidade = model.Cidade,
                        Estado = model.Estado,
                        Data_Nascimento = model.Data_Nascimento
                    };

                    var result = await _userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        return Ok(token);
                    }
                    else
                    {
                        List<IdentityError> erro = new List<IdentityError>();

                        foreach (var error in result.Errors)
                        {
                            erro.Add(error);
                        }
                        return BadRequest(erro.ToArray());
                    }
                }
                return Unauthorized("Usuario(email) ja existe no sistema");
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error API:\n {e}");
            }
        }

        [HttpGet("ConfirmEmailAddress")]
        public async Task<IActionResult> ConfirmEmailAddress(string token, string email)
        {
            if (token.Contains(" "))
            {
                token = token.Replace(" ", "+");
            }
            try
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
                return BadRequest("Erro ao authorizar usuario");
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error API: {e}");
            }
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    return Ok(token);
                }
                return NotFound("Usuario(Email) nao encontrado");
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error API: {e}");
            }
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel modelo)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(modelo.Email);

                if (user != null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, modelo.Token, modelo.Password);

                    if (!result.Succeeded)
                    {
                        return BadRequest("Erro ao authorizar usuario");
                    }
                    return Ok("Senha alterada com sucesso");
                }
                return NotFound("Usuario nao encontrado");
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error API: {e}");
            }
        }
    }
}
