using System;
using System.Collections.Generic;
using System.Net;
using ClinicaHumaita.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaHumaita.Controllers
{
    [ApiController]
    public abstract class MainController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        protected MainController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        protected ActionResult CustomResponse(object result = null)
        {
            try
            {
                if (_notificationService.hasError())
                {
                    var error = _notificationService.getErrors();

                    if(error.StatusCode == HttpStatusCode.NotFound)
                    {
                        return NotFound(new { error = error.Message });
                    }
                    else if (error.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        return Unauthorized(new { error = error.Message });
                    }
                    else
                    {
                        return BadRequest(new
                        {
                            error = _notificationService.getErrors().Message,
                        });
                    }
                }
                else
                {
                    return Ok(result);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    sucesso = false,
                    erros = new List<string> { ex.ToString() }
                });
            }
        }
    }
}
