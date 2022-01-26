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
        
        //alterada a route para nao exibir o /user/
        [Route("/Cadastro")]
        public IActionResult Create()
        {
            //valida se existe um usuario logado
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(Index), "Home");
            }
            //Carrega a View Create
            return View();
        }
        
        //alterada a route para nao exibir o /user/ e validando o token
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/Cadastro")]
        public async Task<IActionResult> Create(User user)
        {
            //valida se o username esta em uso
            var userNameExists = await _service.GetByUserName(user.UserName);
            if (userNameExists != null)
            {
                ModelState.AddModelError("UserName", "Usuário já existe.");
            }

            //valida se o modelstate eh valido
            if (ModelState.IsValid)
            {
                try
                {
                    // Chama a classe de servico e aguarda a execucao
                    await _service.Create(user);
                    //redireciona para Index da PersonController
                    return RedirectToAction(nameof(Login));
                }
                catch
                {
                    //retorna erro em caso de falha na insercao
                    ModelState.AddModelError("UserName", "Problema ao realizar requisição! Atualize a página e tente novamente.");
                }
            }
            //em caso de erro na validacao ou de inserção retorna para a view
            return View(user);
        }
        
        //Editar Usuario
        [Authorize]
        public IActionResult Edit()
        {
            //Busca o username do usuario logado
            var username = User.Claims.Where(x => x.Type == "usersname").FirstOrDefault().Value;

            //valida se o username nao esta vazio
            if (username != null && username != "")
            {
                //busca o usuario logado pelo username
                var user = _service.GetByUserName(username).Result;
                //retorna para a view um model do usuario logado
                return View(user);
            }
            //se o username estiver vazio, redireciona para pagina de login
            return RedirectToAction(nameof(Login));
        }

        //Editar Usuario
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(User user)
        {
            var username = _service.GetByUserName(user.UserName).Result;
            //Busca o username do usuario logado
            var usernameLogged = User.Claims.Where(x => x.Type == "usersname").FirstOrDefault().Value;

            if (username != null && username.UserName != usernameLogged)
            {
                ModelState.AddModelError("UserName", "Usuário já existe.");
            }

            //desloga o usuario
            bool logoff = await LogoutUser();

            //valida se o modelstate eh valido
            if (ModelState.IsValid && logoff)
            {
                try
                {
                    // Chama a classe de servico e aguarda a execucao
                    var newuser = await _service.Edit(user);
                    await LoginUser(newuser);
                    //redireciona para Index da PersonController
                    return RedirectToAction(nameof(Index),"Person");
                }
                catch
                {
                    //retorna erro em caso de falha na insercao
                    ModelState.AddModelError("UserName", "Problema ao realizar requisição! Atualize a página e tente novamente.");
                }
            }

            //em caso de erro na validacao ou de inserção retorna para a view
            return View(user);
        }

        //Editar Usuario
        [Authorize]
        public IActionResult Remove()
        {
            //Busca o username do usuario logado
            var username = User.Claims.Where(x => x.Type == "usersname").FirstOrDefault().Value;

            //busca o usuario logado pelo username
            var user = _service.GetByUserName(username).Result;
            //retorna para a view um model do usuario logado
            return View(user);
        }

        //Editar Usuario
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(User user)
        {
            //busca dados do usuario para ser removido
            var user_to_remove = _service.GetByUserName(user.UserName).Result;
           
            //desloga o usuario
            bool logoff = await LogoutUser();

            //valida usuario foi deslogado
            if (logoff)
            {
                try
                {
                    // Chama a classe de servico e aguarda a execucao
                    var newuser = await _service.Remove(user_to_remove);
                    
                    //redireciona para Index da PersonController
                    return RedirectToAction(nameof(Login));
                }
                catch
                {
                    //retorna erro em caso de falha na insercao
                    ModelState.AddModelError("UserName", "Problema ao realizar requisição! Atualize a página e tente novamente.");
                }
            }

            //em caso de erro na validacao ou de inserção retorna para a view
            return View(user);
        }

        //alterada a route para nao exibir o /user/
        [Route("/Login")]
        public IActionResult Login()
        {
            //valida se existe um usuario logado
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(Index), "Home");
            }

            //Carrega a View Login
            return View();
        }
        //alterada a route para nao exibir o /user/ e validando o token
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/Login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            //valida se o modelstate eh valido
            if (ModelState.IsValid)
            {
                try
                {
                    // Chama a classe de servico e aguarda a execucao do metodo de login
                    var user = await _service.Login(username, password);
                    //se o retorno for null, o usuario nao existe na base ou informou dados incorretos
                    if (user == null)
                    {
                        ModelState.AddModelError("Password", "Usuário ou senha inválidos.");
                    }
                    else
                    {

                        if (LoginUser(user).Result == true)
                        {
                            //redireciona para a pagina principal quando usuario logado
                            return RedirectToAction(nameof(Index), "Person");
                        }
                    }
                }
                catch
                {
                    //retorna erro em caso de falha no login
                    ModelState.AddModelError("UserName", "Problema ao realizar requisição! Atualize a página e tente novamente.");
                }
            }
            //em caso de erro na validacao retorna para a view
            return View();
        }
        
        //realiza o logout do usuario
        [HttpGet]
        public async Task<IActionResult>  Logout()
        {
            //realiza o logout do usuario
            await LogoutUser();

            //redireciona para o login
            return RedirectToAction(nameof(Login));
        }
        
        //seta o login do usuario no claim
        private async Task<bool> LoginUser(User user)
        {
            try
            {
                //se o usuario estiver logado, realiza o logout
                if (User.Identity.IsAuthenticated)
                {
                     await LogoutUser();
                }

                //se o usuario for valido, faz login do usuario 
                var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Person.name), new Claim("usersname", user.UserName) };

                ClaimsIdentity userIdentity = new ClaimsIdentity(claims, "login");
                ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                await HttpContext.SignInAsync(principal);
                return true;
            }catch
            {
                return false;
            }
        }
        
        //seta o logout do usuario no claim
        private async Task<bool> LogoutUser()
        {
            await HttpContext.SignOutAsync();
            return true;
        }
    }
}
