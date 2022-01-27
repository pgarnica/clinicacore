using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ClinicaHumaita.Business.Interfaces;
using ClinicaHumaita.Data.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaHumaita.Controllers
{
    public class UserController : Controller
    {
        //instancia o servico de usuario para ser utilizado pela controller e evitar acesso direto aos dados.
        private readonly IUserService _service;
        public UserController(IUserService service)
        {
            _service = service;
        }

        //alterada a route para nao exibir o /user/ e validando o token
        [HttpPost]
        [Route("/Cadastro")]
        public async Task<ActionResult<User>> Create(User user)
        {
            try
            {
                return Ok(await _service.Create(user));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        //Editar Usuario
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<User>> Edit(User user)
        {
            try
            {
                return Ok(await _service.Edit(user));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        //Editar Usuario
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Remove(User user)
        {
            try
            {
                return Ok(await _service.Remove(user));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
