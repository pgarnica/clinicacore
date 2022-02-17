using ClinicaHumaita.Business.Interfaces;
using ClinicaHumaita.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace ClinicaHumaita.Services
{
    public class NotificationService : INotificationService
    {

        private List<Error> _errors;
        public NotificationService()
        {
            _errors = new List<Error>();
        }
        public void Add(Error Error)
        {
            _errors.Add(Error);
        }

        public Error getErrors()
        {
            return _errors.First();
        }

        public bool hasError()
        {
            return _errors.Count > 0 ;
        }
    }
}
