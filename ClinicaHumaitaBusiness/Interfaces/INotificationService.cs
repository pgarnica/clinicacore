using ClinicaHumaita.Data.Models;

namespace ClinicaHumaita.Business.Interfaces
{
    public interface INotificationService
    {
        void Add(Error error);
        bool hasError();
        Error getErrors();
    }
}
