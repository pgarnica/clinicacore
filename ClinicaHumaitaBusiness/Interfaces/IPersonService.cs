using ClinicaHumaita.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClinicaHumaita.Business.Interfaces
{
    public interface IPersonService
    {
        Task<Person> Add(Person newPerson);
        Task<Person> Update(Person person);
        Task<bool> Delete(Person person);
        Task<Person> GetById(int? id);
        Task<List<Person>> GetPersons();
    }
}
