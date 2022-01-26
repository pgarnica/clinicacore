using ClinicaHumaita.Data.Context;
using ClinicaHumaita.Data.Interfaces;
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
            var repository = await CreateRepositoryAsync(true);
            var personService = new PersonService(repository);

            //Act
            var personRetorno = await personService.GetById(1);

            //Assert
            Assert.NotNull(personRetorno);
            Assert.Equal(1, personRetorno.id);
        }

        [Fact]
        public async void PersonService_GetUsersPersons()
        {
            var repository = await CreateRepositoryAsync(true);
            var personService = new PersonService(repository);

            //Act
            var personRetorno = await personService.GetUsersPersons();

            //Assert
            Assert.NotNull(personRetorno);
            Assert.Equal(2,personRetorno.Count);
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

            var repository = await CreateRepositoryAsync();
            var personService = new PersonService(repository);

            //Act
            await personService.Create(person);

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
                id = 3,
                name = "Paulo Garnica",
                email = "paulo.garnica@gmail.com"
            };

            var repository = await CreateRepositoryAsync(true);
            var personService = new PersonService(repository);

            //Act
            var personOriginal = await personService.GetById(3);

            await personService.Edit(person);

            var personAlterada = await personService.GetById(3);

            //Assert
            Assert.NotNull(personAlterada);
            Assert.Equal(personAlterada.id, personOriginal.id);
            Assert.NotEqual(personAlterada.name, personOriginal.name);
            Assert.NotEqual(personAlterada.email, personOriginal.email);
        }

        [Fact]
        public async void PersonService_Remove()
        {
            //Arrange
            var person = new Person
            {
                id = 3,
            };

            var repository = await CreateRepositoryAsync(true);
            var personService = new PersonService(repository);

            //Act
            var personAntes = await personService.GetById(3);
            await personService.Remove(person);
            var personRemover = await personService.GetById(3);

            //Assert
            Assert.Null(personRemover);
            Assert.NotNull(personAntes);
        }

        private async Task<PersonRepository> CreateRepositoryAsync(bool populated = false)
        {
            ClinicaContext context = new ClinicaContext(_options);

            if (populated)
            {
                await PopulateDataAsync(context);
            }
            return new PersonRepository(context);
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
                        PersonId = person.id,
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
