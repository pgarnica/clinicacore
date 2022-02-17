using System;
using System.Threading.Tasks;
using ClinicaHumaita.Business.Interfaces;
using ClinicaHumaita.Data.Models;
using ClinicaHumaita.Shared.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaHumaita.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : MainController
    {
        //instancia o servico de usuario para ser utilizado pela controller e evitar acesso direto aos dados.
        private readonly IAuthenticationService _authenticationService;
        public AuthenticationController(IAuthenticationService authenticationService,
                                INotificationService notificationService) : base(notificationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserTokenViewModel>> Login([FromBody] LoginViewModel login)
        {
            try 
            { 
                return CustomResponse(await _authenticationService.Authenticate(login));
            }
            catch (Exception ex)
            {
                return BadRequest(new { errorMessage = ex.Message });
            }
        }
    }
}
