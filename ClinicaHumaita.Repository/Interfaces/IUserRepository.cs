using ClinicaHumaita.Data.Models;
using System;
using System.Threading.Tasks;

namespace ClinicaHumaita.Data.Interfaces
{
    public interface IUserRepository : IDisposable
    {
        Task<User> Create(User user);
        Task<User> Edit(User user);
        Task<User> Remove(User user);
        Task<User> Login(string username, string password);
        Task<User> GetByUserName(string username);
        Task<bool> PersonIsUser(int personId);
    }
}
