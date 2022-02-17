using System;
using System.Threading.Tasks;
using ClinicaHumaita.Business.Interfaces;
using ClinicaHumaita.Shared.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaHumaita.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : MainController
    {
        private readonly IPersonService _personService;
        public PersonController(IPersonService personService, 
                                INotificationService notificationService)  : base(notificationService)
        {
            _personService = personService;
        }

        [Authorize]
        [HttpGet("list-persons")]
        public async Task<ActionResult> ListPersons()
        {
            try
            {
                return CustomResponse(await _personService.GetPersons());
            }
            catch (Exception ex)
            {
                return BadRequest(new { errorMessage = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("Add")]
        public async Task<ActionResult> Add([FromBody]PersonAddViewModel personAdd)
        {
            try
            {
                return CustomResponse(await _personService.Add(personAdd));
            }
            catch (Exception ex)
            {
                return BadRequest(new {errorMessage = ex.Message});
            }
        }

        [Authorize]
        [HttpPut("Update")]
        public async Task<ActionResult> Update([FromBody]PersonUpdateViewModel personUpdate)
        {
            try
            {
                return CustomResponse(await _personService.Update(personUpdate));
            }
            catch (Exception ex)
            {
                return BadRequest(new { errorMessage = ex.Message });
            }
        }

        [Authorize]
        [HttpDelete("Delete")]
        public async Task<ActionResult> Delete([FromBody] PersonDeleteViewModel personDelete)
        {
            try
            {
                return CustomResponse(await _personService.Delete(personDelete));
            }
            catch(Exception ex)
            {
                return BadRequest(new { errorMessage = ex.Message });
            }
        }

        //mostrar detalhes da pessoa
        [Authorize]
        [HttpGet("Detail")]
        public async Task<ActionResult> Detail([FromQuery] int? id)
        {
            try
            {
                return CustomResponse(await _personService.GetById(id));
            }
            catch (Exception ex)
            {
                return BadRequest(new { errorMessage = ex.Message });
            }
        }
    }
}
