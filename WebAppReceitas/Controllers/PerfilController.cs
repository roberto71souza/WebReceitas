﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAppReceitas.Models;
using WebAppReceitas.Services;

namespace WebAppReceitas.Controllers
{
    public class PerfilController : Controller
    {
        public ReceitaService _receitaService { get; set; }
        public IToastNotification _toast { get; }
        public IMapper _mapper { get; set; }
        public PerfilController(ReceitaService receita, IToastNotification toast, IMapper mapper)
        {
            _mapper = mapper;
            _receitaService = receita;
            _toast = toast;
        }

        [HttpGet]
        public async Task<ActionResult<ReceitaModel>> Perfil(int id)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    id = int.Parse(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
                    var receitaUsuario = await _receitaService.ListarReceitasUsuarioId(id);

                    var mapResult = _mapper.Map<ReceitaModel[]>(receitaUsuario);
                    return View(mapResult);
                }
                return RedirectToAction("Login", "LoginUsuario");
            }
            catch (Exception)
            {
                return RedirectToAction("HttpStatusCodeHandler", "Home", new { statusCode = this.Response.StatusCode = 500 });
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
    }
}
