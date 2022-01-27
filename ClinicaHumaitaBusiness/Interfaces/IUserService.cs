using ClinicaHumaita.Data.Models;
using ClinicaHumaita.Shared.ViewModels;
using System.Threading.Tasks;

namespace ClinicaHumaita.Business.Interfaces
{
    public interface IUserService
    {
        Task<User> Add(UserAddViewModel userAdd);
        Task<User> Update(UserUpdateViewModel userUpdate);
        Task<User> Delete(UserDeleteViewModel user);
        Task<User> ValidateUser(string username, string password);
        Task<User> GetByUserName(string username);
    }
}
