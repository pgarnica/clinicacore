﻿using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ClinicaHumaita.Business.Interfaces;
using ClinicaHumaita.Data.Models;
using ClinicaHumaita.Business.Configuration;
using ClinicaHumaita.Shared.ViewModels;
using System.Threading.Tasks;
using System.Net;

namespace ClinicaHumaita.Services
{
    public class AuthenticationService : BaseService, IAuthenticationService
    {
        private readonly IUserService _userService;
        public AuthenticationService(IUserService userService,
                           INotificationService notificationService) : base(notificationService)
        {
            _userService = userService;
        }

        public async Task<UserTokenViewModel> Authenticate(LoginViewModel login)
        {
            // Recupera o usuário
            var validUser = await _userService.ValidateUser(login.UserName, login.Password);

            // Verifica se o usuário existe
            if (validUser == null) 
            {
                ErrorNotification(HttpStatusCode.Unauthorized, "Invalid Username or Password.");
                return null;
            }

            // Gera o Token
            var token = GenerateToken(validUser);

            // Retorna os dados
            return new UserTokenViewModel
            {
                UserName = validUser.UserName,
                Token = token
            };
        }

        private static string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Settings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Person.email)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
