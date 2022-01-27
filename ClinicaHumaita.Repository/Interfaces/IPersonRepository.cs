using ClinicaHumaita.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClinicaHumaita.Data.Interfaces
{
    public interface IPersonRepository : IDisposable
    {
        Task<Person> Add(Person newPerson);
        Task<Person> Update(Person person);
        Task<bool> Delete(Person person);
        Task<Person> GetById(int id);
        Task<List<Person>> GetPersons();
        Task<bool> ValidateUniqueEmail(Person person);
    }
}
