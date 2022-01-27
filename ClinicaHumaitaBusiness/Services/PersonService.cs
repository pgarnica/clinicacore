using ClinicaHumaita.Business.Interfaces;
using ClinicaHumaita.Data.Interfaces;
using ClinicaHumaita.Data.Models;
using ClinicaHumaita.Shared.ViewModels;
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
        public readonly IUserRepository _userRepository;
        public PersonService(IPersonRepository personRepository,
                             IUserRepository userRepository)
        {
            _personRepository = personRepository;
            _userRepository = userRepository;
        }
        public async Task<Person> Add(Person newPerson)
        {
            try
            {
                var personValidation = await ValidPerson(newPerson);
                if(!personValidation.Valid)
                {
                    throw new Exception(personValidation.Message);
                }

                //retornar o objeto que foi salvo
                return await _personRepository.Add(newPerson);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public async Task<Person> Update(Person person)
        {
            try
            {
                if (!person.id.HasValue)
                {
                    throw new Exception("The id field is required.");
                }

                var personEdit = await _personRepository.GetById(person.id.Value);
                if (personEdit == null)
                {
                    throw new Exception("Person not found.");
                }

                var personValidation = await ValidPerson(person);
                if (!personValidation.Valid)
                {
                    throw new Exception(personValidation.Message);
                }

                personEdit.name = person.name;
                personEdit.email = person.email;

                return await _personRepository.Update(personEdit);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> Delete(Person person)
        {
            try
            {
                if (!person.id.HasValue)
                {
                    throw new Exception("The id field is required.");
                }

                var personRemove = await _personRepository.GetById(person.id.Value);
                if (personRemove == null)
                {
                    throw new Exception("Person not found.");
                }

                if (await _userRepository.PersonIsUser(personRemove.id.Value))
                {
                    throw new Exception("This person has an user and can't be deleted.");
                }

                return await _personRepository.Delete(personRemove);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<Person> GetById(int? id)
        {
            try
            {
                if (!id.HasValue)
                {
                    throw new Exception("The id field is required.");
                }

                var person = await _personRepository.GetById(id.Value);
                if (person == null)
                {
                    throw new Exception("Person not found.");
                }
                return person;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<Person>> GetPersons()
        {
            try
            {
                //retorna uma lista de person que sao users
                return await _personRepository.GetPersons();
            }
            catch
            {
                // retorna uma exception em caso de falha
                throw new InvalidDataException();
            }
        }
        private async Task<ValidationViewModel> ValidPerson(Person person)
        {
            var validation = new ValidationViewModel
            {
                Valid = true,
                Message = String.Empty
            };
            
            if (string.IsNullOrEmpty(person.name))
            {
                validation.Valid = false;
                validation.Message = "Name field is required.";
            }

            if (string.IsNullOrEmpty(person.email))
            {
                validation.Valid = false;
                validation.Message = "Email field is required.";
            }

            if (await _personRepository.ValidateUniqueEmail(person))
            {
                validation.Valid = false;
                validation.Message = "There is already a person with the given email.";
            }
            
            return validation;
        }

        public void Dispose()
        {
            _personRepository?.Dispose();
            _userRepository?.Dispose();
        }
    }
}
