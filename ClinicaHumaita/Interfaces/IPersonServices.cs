using ClinicaHumaita.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicaHumaita.Interfaces
{
    public interface IPersonServices
    {
        Task<Person> Add(Person newItem);
        Task<Person> GetById(int id);
        Task<List<Person>> Get();
    }
}
