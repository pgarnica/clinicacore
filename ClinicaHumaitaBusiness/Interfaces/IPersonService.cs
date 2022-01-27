using ClinicaHumaita.Data.Models;
using ClinicaHumaita.Shared.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClinicaHumaita.Business.Interfaces
{
    public interface IPersonService
    {
        Task<Person> Add(PersonAddViewModel personAdd);
        Task<Person> Update(PersonUpdateViewModel personUpdate);
        Task<bool> Delete(PersonDeleteViewModel personDelete);
        Task<Person> GetById(int? id);
        Task<List<Person>> GetPersons();
    }
}
