using ClinicaHumaita.Business.Interfaces;
using ClinicaHumaita.Data.Models;
using System.Collections.Generic;
using System.Net;

namespace ClinicaHumaita.Services
{
    public abstract class BaseService
    {
        private readonly INotificationService _notificationService;
        protected BaseService(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        protected void ErrorNotification(HttpStatusCode statusCode, string message )
        {
            _notificationService.Add(new Error(statusCode,message));
        }

        protected Error GetErrors()
        {
           return _notificationService.getErrors();
        }
    }
}
