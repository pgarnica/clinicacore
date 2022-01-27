using ClinicaHumaita.Data.Context;
using ClinicaHumaita.Data.Models;
using ClinicaHumaita.Data.Repository;
using ClinicaHumaita.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ClinicaHumaita.Tests.Services
{
    public class PersonServiceTests
    {
        private DbContextOptions<ClinicaContext> _options;

        public PersonServiceTests()
        {
            _options = new DbContextOptionsBuilder<ClinicaContext>().UseInMemoryDatabase(databaseName: "TestNewListDb").Options;
        }

        [Fact]
        public async void PersonService_GetById()
        {
            var personRepository = await CreatePersonRepositoryAsync(false);
            var userRepository = await CreateUserRepositoryAsync(true);
            var personService = new PersonService(personRepository, userRepository);

            //Act
            var personRetorno = await personService.GetById(1);

            //Assert
            Assert.NotNull(personRetorno);
            Assert.Equal(1, personRetorno.id);
        }

        [Fact]
        public async void PersonService_GetPersons()
        {
            var personRepository = await CreatePersonRepositoryAsync(true);
            var userRepository = await CreateUserRepositoryAsync(false);
            var personService = new PersonService(personRepository, userRepository);

            //Act
            var personRetorno = await personService.GetPersons();

            //Assert
            Assert.NotNull(personRetorno);
            Assert.Equal(3,personRetorno.Count);
        }

        [Fact]
        public async void PersonService_Create()
        {
            //Arrange
            var person = new Person
            {
                id = 10,
                name = "Paulo Garnica",
                email = "paulo.garnica@gmail.com"
            };

            var personRepository = await CreatePersonRepositoryAsync(false);
            var userRepository = await CreateUserRepositoryAsync(true);
            var personService = new PersonService(personRepository, userRepository);

            //Act
            await personService.Add(person);

            var personRetorno = await personService.GetById(person.id);

            //Assert
            Assert.NotNull(personRetorno);
            Assert.Equal(person.id, personRetorno.id);
            Assert.Equal(person.name, personRetorno.name);
            Assert.Equal(person.email, personRetorno.email);
        }

        [Fact]
        public async void PersonService_Edit()
        {
            //Arrange
            var person = new Person
            {
                id = 1,
                name = "Paulo Garnica",
                email = "paulo.garnica@gmail.com"
            };

            var personRepository = await CreatePersonRepositoryAsync(false);
            var userRepository = await CreateUserRepositoryAsync(true);
            var personService = new PersonService(personRepository, userRepository);

            //Act
            await personService.Update(person);

            var personAlterada = await personService.GetById(1);

            //Assert
            Assert.NotNull(personAlterada);
            Assert.Equal(personAlterada.id, person.id);
            Assert.Equal(personAlterada.name, person.name);
            Assert.Equal(personAlterada.email, person.email);
        }

        [Fact]
        public async void PersonService_Remove()
        {
            //Arrange
            var person = new Person
            {
                id = 1,
            };

            var personRepository = await CreatePersonRepositoryAsync(true);
            var userRepository = await CreateUserRepositoryAsync(false);
            var personService = new PersonService(personRepository, userRepository);

            //Act
            var retorno = await personService.Delete(person);

            //Assert
            Assert.True(retorno);
        }

        private async Task<PersonRepository> CreatePersonRepositoryAsync(bool populated = false)
        {
            ClinicaContext context = new ClinicaContext(_options);

            if (populated)
            {
                await PopulateDataAsync(context);
            }
            return new PersonRepository(context);
        }

        private async Task<UserRepository> CreateUserRepositoryAsync(bool populated = false)
        {
            ClinicaContext context = new ClinicaContext(_options);

            if (populated)
            {
                await PopulateDataAsync(context);
            }
            return new UserRepository(context);
        }

        private async Task PopulateDataAsync(ClinicaContext context)
        {
            int index = 1;

            while (index <= 3)
            {
                var person = new Person()
                {
                    id = index,
                    name = $"Person_{index}",
                    email = $"Person_{index}@email.com.br"
                 
                };

                await context.Person.AddAsync(person);

                if (index % 2 == 0)
                {
                    User user = new User
                    {
                        PersonId = person.id.Value,
                        Active = true,
                        Creation_Date  = DateTime.Now,
                        Last_login = null,
                        Password = person.name,
                        UserName = person.email
                    };

                    await context.Users.AddAsync(user);
                }
                index++;
            }

            await context.SaveChangesAsync();
        }
    }
}
