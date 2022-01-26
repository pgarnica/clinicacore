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
    public class PersonController : Controller
    {
        //instacia do servico de pessoas para evitar acesso diretos aos dados
        private readonly IPersonService _service;
        public PersonController(IPersonService service)
        {
            _service = service;
        }

        //carrega lista de pessoas
        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await _service.GetUsersPersons());
        }

        //adicionar uma pessoa
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

        //editar uma pessoa
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {

            if(id == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var pessoa = await _service.GetById(int.Parse(id.ToString()));
                
            return View(pessoa);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Person person)
        {
            try
            {
                if (ModelState.IsValid)
                {   //adiciona person
                    var result = await _service.Edit(person);
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

        //remover uma pessoa
        [Authorize]
        public async Task<IActionResult> Remove(int? id)
        {

            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var pessoa = await _service.GetById(int.Parse(id.ToString()));

            return View(pessoa);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Remove(Person person)
        {
            try
            {
                  //remove person
                    var result = await _service.Remove(person);
                    //redireciona para lista de persons
                    return RedirectToAction(nameof(Index));
               
            }
            catch
            {
                //em caso de erro retorno uma excpetion.
                throw new InvalidDataException();
            }
        }

        //mostrar detalhes da pessoa
        [Authorize]
        public async Task<IActionResult> Detail(int? id)
        {

            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var pessoa = await _service.GetById(int.Parse(id.ToString()));

            return View(pessoa);
        }
    }
}
