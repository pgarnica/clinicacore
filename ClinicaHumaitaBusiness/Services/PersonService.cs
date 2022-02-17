using AutoMapper;
using ClinicaHumaita.Business.Interfaces;
using ClinicaHumaita.Business.Validation;
using ClinicaHumaita.Data.Interfaces;
using ClinicaHumaita.Data.Models;
using ClinicaHumaita.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ClinicaHumaita.Services
{
    public class PersonService : BaseService, IPersonService
    {
        private readonly IPersonRepository _personRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRabbitMQService _rabbitMQService;
        private readonly IMapper _mapper;

        public PersonService(IPersonRepository personRepository,
                             IUserRepository userRepository,
                             IMapper mapper,
                             IRabbitMQService rabbitMQService,
                             INotificationService notificationService) : base(notificationService)
        {
            _personRepository = personRepository;
            _userRepository = userRepository;
            _rabbitMQService = rabbitMQService;
            _mapper = mapper;
        }
        public async Task<Person> Add(PersonAddViewModel personAdd)
        {
            try
            {
                var newPerson =  _mapper.Map<Person>(personAdd);

                if(!await ValidPerson(newPerson))
                {
                    return null;
                }
              
                var person = await _personRepository.Add(newPerson);

                if(person != null)
                {
                   await _rabbitMQService.Send(String.Format("Adicionada Pessoa {0}", person.name));
                }

                //retornar o objeto que foi salvo
                return person;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public async Task<Person> Update(PersonUpdateViewModel personUpdate)
        {
            try
            {
                if (!personUpdate.Id.HasValue)
                {
                    ErrorNotification(HttpStatusCode.BadRequest, "The id field is required.");
                }

                var personEdit = await _personRepository.GetById(personUpdate.Id.Value);
                if (personEdit == null)
                {
                    ErrorNotification(HttpStatusCode.NotFound, "Person not found.");
                }

                personEdit.name = personUpdate.Name;
                personEdit.email = personUpdate.Email;

                if (!await ValidPerson(personEdit))
                {
                    return null;
                }

                return await _personRepository.Update(personEdit);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> Delete(PersonDeleteViewModel personDelete)
        {
            try
            {
                if (!personDelete.Id.HasValue)
                {
                    ErrorNotification(HttpStatusCode.BadRequest, "The id field is required.");
                }

                var personRemove = await _personRepository.GetById(personDelete.Id.Value);
                if (personRemove == null)
                {
                    ErrorNotification(HttpStatusCode.NotFound, "Person not found.");
                }

                if (await _userRepository.PersonIsUser(personRemove.id.Value))
                {
                    ErrorNotification(HttpStatusCode.BadRequest, "This person has an user and can't be deleted.");
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
                    ErrorNotification(HttpStatusCode.BadRequest, "The id field is required.");
                }

                var person = await _personRepository.GetById(id.Value);

                if(person == null)
                {
                    ErrorNotification(HttpStatusCode.NotFound, "Person not found.");
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
                return await _personRepository.GetPersons();
            }
            catch
            {
                throw new InvalidDataException();
            }
        }
        private async Task<bool> ValidPerson(Person person)
        {
             
            PersonValidation validation = new PersonValidation();

            var validationResult = validation.Validate(person);
            if (!validationResult.IsValid)
            {
                ErrorNotification(HttpStatusCode.BadRequest, validationResult.Errors.Select(x => x.ErrorMessage).FirstOrDefault());
                return false;
            }

            if (await _personRepository.ValidateUniqueEmail(person))
            {
                ErrorNotification(HttpStatusCode.BadRequest, "There is already a person with the given email.");
                return false;
            }
            
            return true;
        }
        public void Dispose()
        {
            _personRepository?.Dispose();
            _userRepository?.Dispose();
        }
    }
}
