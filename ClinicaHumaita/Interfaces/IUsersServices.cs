using ClinicaHumaita.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicaHumaita.Interfaces
{
    public interface IUsersServices
    {
        Task<Users> Create(Users user);
        Task<Users> Edit(Users user);
        Task<Users> Remove(Users user);
        Task<Users> Login(string username, string password);
        Task<Users> GetByUserName(string username);
        string MD5Hash(string text);
    }
}
