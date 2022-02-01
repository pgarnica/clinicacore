using ClinicaHumaita.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClinicaHumaita.Data.Interfaces
{
    public interface ILogRepository : IDisposable
    {
        Task<Log> Add(Log newlog);
    }
}
