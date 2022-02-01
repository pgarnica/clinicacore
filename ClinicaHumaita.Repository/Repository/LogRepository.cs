using ClinicaHumaita.Data.Context;
using ClinicaHumaita.Data.Interfaces;
using ClinicaHumaita.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicaHumaita.Data.Repository
{
    public class LogRepository : ILogRepository
    {
        private readonly ClinicaContext _db;
        public ClinicaContext DbContext { get; private set; }
        public LogRepository(ClinicaContext db)
        {
            _db = db;
        }
        public async Task<Log> Add(Log newLog)
        {
            try
            {
                //salvar no banco
                await _db.Logs.AddAsync(newLog);
                await _db.SaveChangesAsync();
                //retornar o objeto que foi salvo
                return newLog;
            }
            catch(Exception ex)
            {
                // retorna uma exception em caso de falha na insercao
                throw ex;
            }
        }
        public void Dispose()
        {
            _db?.Dispose();
        }
    }
}
