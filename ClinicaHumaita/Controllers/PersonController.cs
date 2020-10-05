using System;
using System.Collections.Generic;
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
        public ActionResult Create(Person person)
        {
            try
            {
                _service.Add(person);  
            }
            catch
            {
                return View();
            }

            return View();
        }

    }
}
