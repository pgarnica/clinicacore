using System;

namespace ClinicaHumaita.Data.Models
{
    public class Log
    {
        public Guid Id { get; set; }
        public DateTime Datahora { get; set; }
        public string Descricao { get; set; }
    }
}
