using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClinicaHumaita.Business.Interfaces;
using ClinicaHumaita.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaHumaita.Controllers
{
    [Route("api/[controller]")]
    public class PersonController : Controller
    {
        private readonly IPersonService _personService;
        public PersonController(IPersonService personService)
        {
            _personService = personService;
        }

        [Authorize]
        [HttpGet("list-persons")]
        public async Task<ActionResult<List<Person>>> ListPersons()
        {
            try
            {
                return Ok(await _personService.GetPersons());
            }
            catch (Exception ex)
            {
                return BadRequest(new { errorMessage = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("Add")]
        public async Task<ActionResult<Person>> Add([FromBody]Person person)
        {
            try
            {
                return Ok(await _personService.Add(person));
            }
            catch (Exception ex)
            {
                return BadRequest(new {errorMessage = ex.Message});
            }
        }

        [Authorize]
        [HttpPut("Update")]
        public async Task<ActionResult<Person>> Update([FromBody] Person person)
        {
            try
            {
                return Ok(await _personService.Update(person));
            }
            catch (Exception ex)
            {
                return BadRequest(new { errorMessage = ex.Message });
            }
        }

        [Authorize]
        [HttpDelete("Delete")]
        public async Task<ActionResult<bool>> Delete([FromBody] Person person)
        {
            try
            {
                return Ok(await _personService.Delete(person));
            }
            catch(Exception ex)
            {
                return BadRequest(new { errorMessage = ex.Message });
            }
        }

        //mostrar detalhes da pessoa
        [Authorize]
        [HttpGet("Detail")]
        public async Task<ActionResult<Person>> Detail([FromQuery] int? id)
        {
            try
            {
                return Ok(await _personService.GetById(id));
            }
            catch (Exception ex)
            {
                return BadRequest(new { errorMessage = ex.Message });
            }
        }
    }
}
