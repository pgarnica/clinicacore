using ClinicaHumaita.Data.Models;
using System.Threading.Tasks;

namespace ClinicaHumaita.Business.Interfaces
{
    public interface IUserService
    {
        Task<User> Create(User user);
        Task<User> Edit(User user);
        Task<User> Remove(User user);
        Task<User> Login(string username, string password);
        Task<User> GetByUserName(string username);
    }
}
