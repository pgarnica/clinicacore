using ClinicaHumaita.Business.Interfaces;
using ClinicaHumaita.Data.Interfaces;
using ClinicaHumaita.Data.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicaHumaita.Services
{
    public class PersonService : IPersonService
    {
        public readonly IPersonRepository _personRepository;
        public PersonService(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }
        public async Task<Person> Create(Person newItem)
        {
            try
            {
                //retornar o objeto que foi salvo
                return await _personRepository.Create(newItem);
            }
            catch
            {
                // retorna uma exception em caso de falha na insercao
                throw new InvalidDataException();
            }
        }
        public async Task<Person> Edit(Person person)
        {
            try
            {
                //retornar o objeto que foi salvo
                return await _personRepository.Edit(person);
            }
            catch(Exception ex)
            {
                // retorna uma exception em caso de falha na alteracao
                throw ex;
            }
        }
        public async Task<bool> Remove(Person person)
        {
            try
            {
               return await _personRepository.Remove(person);
            }
            catch
            {
                // retorna uma exception em caso de falha na remocao
                throw new InvalidDataException();
            }
        }
        public async Task<Person> GetById(int id)
        {
            try
            {
                //retorna person pelo id
                return await _personRepository.GetById(id);
            }
            catch
            {
                // retorna uma exception em caso de falha
                throw new InvalidDataException();
            }
        }
        public async Task<List<Person>> GetUsersPersons()
        {
            try
            {
                //retorna uma lista de person que sao users
                return await _personRepository.GetUsersPersons();
            }
            catch
            {
                // retorna uma exception em caso de falha
                throw new InvalidDataException();
            }
        }
    }
}
