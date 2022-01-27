using ClinicaHumaita.Data.Models;
using ClinicaHumaita.Shared.ViewModels;
using System.Threading.Tasks;

namespace ClinicaHumaita.Business.Interfaces
{
    public interface IAuthenticationService
    {
        Task<UserTokenViewModel> Authenticate(User user);
    }
}
