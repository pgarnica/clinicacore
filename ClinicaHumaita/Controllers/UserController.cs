using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ClinicaHumaita.Business.Interfaces;
using ClinicaHumaita.Data.Models;
using ClinicaHumaita.Shared.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaHumaita.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        //instancia o servico de usuario para ser utilizado pela controller e evitar acesso direto aos dados.
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Add")]
        public async Task<ActionResult> Add([FromBody]UserAddViewModel userAdd)
        {
            try
            {
                return Ok(await _userService.Add(userAdd));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        //Editar Usuario
        [Authorize]
        [HttpPut("Update")]
        public async Task<ActionResult> Update([FromBody]UserUpdateViewModel userUpdate)
        {
            try
            {
                return Ok(await _userService.Update(userUpdate));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        //Editar Usuario
        [Authorize]
        [HttpDelete("Delete")]
        public async Task<ActionResult> Delete([FromBody]UserDeleteViewModel user)
        {
            try
            {
                return Ok(await _userService.Delete(user));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
