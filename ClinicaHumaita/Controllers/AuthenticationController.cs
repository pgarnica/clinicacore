using System;
using System.Threading.Tasks;
using ClinicaHumaita.Business.Interfaces;
using ClinicaHumaita.Data.Models;
using ClinicaHumaita.Shared.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaHumaita.Controllers
{
    [Route("api/[controller]")]
    public class AuthenticationController : Controller
    {
        //instancia o servico de usuario para ser utilizado pela controller e evitar acesso direto aos dados.
        private readonly IAuthenticationService _authenticationService;
        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserTokenViewModel>> Login([FromBody] User user)
        {
            try 
            { 
                return Ok(await _authenticationService.Authenticate(user));
            }
            catch (Exception ex)
            {
                return BadRequest(new { errorMessage = ex.Message });
            }
        }
    }
}
