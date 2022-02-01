using ClinicaHumaita.Data.Models;
using ClinicaHumaita.Shared.ViewModels;
using System.Threading.Tasks;

namespace ClinicaHumaita.Business.Interfaces
{
    public interface ILogService
    {
        Task<Log> Add(LogAddViewModel userAdd);
    }
}
