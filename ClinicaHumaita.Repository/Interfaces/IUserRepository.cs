using ClinicaHumaita.Data.Models;
using System;
using System.Threading.Tasks;

namespace ClinicaHumaita.Data.Interfaces
{
    public interface IUserRepository : IDisposable
    {
        Task<User> Add(User user);
        Task<User> Update(User user);
        Task<User> Delete(User user);
        Task<User> Login(string username, string password);
        Task<User> GetByUserName(string username);
        Task<bool> PersonIsUser(int personId);
        Task<bool> CheckExistingUserName(string username, int? id);
        Task<User> GetById(int id);
    }
}
