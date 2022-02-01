using ClinicaHumaita.Business.Interfaces;
using ClinicaHumaita.Data.Interfaces;
using ClinicaHumaita.Data.Models;
using ClinicaHumaita.Shared.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaHumaita.Services
{
    public class LogService : ILogService
    {
        private readonly ILogRepository _logRepository;
        public LogService(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }
        public async Task<Log> Add(LogAddViewModel newLog)
        {
            try
            { 
                return await _logRepository.Add(new Log { Id = Guid.NewGuid(), Datahora = DateTime.UtcNow, Descricao = newLog.Descricao});
            }
            catch(Exception ex) 
            {
                throw ex;
            }
        }
       
    }
}
