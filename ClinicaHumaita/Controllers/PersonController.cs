using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClinicaHumaita.Interfaces;
using ClinicaHumaita.Models;
using ClinicaHumaita.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaHumaita.Controllers
{
    public class PersonController : Controller
    {

        private readonly IPersonServices _service;
        public PersonController(IPersonServices service)
        {
            _service = service;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await _service.Get());
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Person person)
        {
            try
            {
                if(ModelState.IsValid)
                {   //adiciona person
                    var result = await _service.Create(person);
                    //redireciona para lista de persons
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    //se a modelstate nao for valida retorna erro
                    ModelState.AddModelError("email", "Dados inválidos");
                    return View(person);
                }
            }
            catch
            {
                //em caso de erro retorno uma excpetion.
                throw new InvalidDataException();
            }
        }

    }
}
