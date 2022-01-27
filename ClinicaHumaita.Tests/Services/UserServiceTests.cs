using ClinicaHumaita.Business.Interfaces;
using ClinicaHumaita.Data.Context;
using ClinicaHumaita.Data.Models;
using ClinicaHumaita.Data.Repository;
using ClinicaHumaita.Services;
using ClinicaHumaita.Shared.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ClinicaHumaita.Tests.Services
{
    public class UserServiceTests
    {
        private DbContextOptions<ClinicaContext> _options;
         
        public UserServiceTests()
        {
            _options = new DbContextOptionsBuilder<ClinicaContext>().UseInMemoryDatabase(databaseName: "TestNewListDb").Options;
            
        }

        [Fact]
        public async void PersonService_Add()
        {
            //Arrange
            var user = new UserAddViewModel
            {
                Password = "teste123",
                Username = "garnica",
                Name = "Paulo Garnica",
                Email = "paulo.garnica@gmail.com"
            };

            var repository = await CreateRepositoryAsync(false);
            var personRepository = await CreatePersonRepositoryAsync(false);
            var personService = new PersonService(personRepository, repository);
            var userService = new UserService(repository, personService);

            //Act
            await userService.Add(user);

            var userRetorno = await userService.GetByUserName(user.Username);

            //Assert
            Assert.NotNull(userRetorno);
            Assert.Equal(user.Username, userRetorno.UserName);
           
        }

        [Fact]
        public async void PersonService_GetByUserName()
        {
            //Arrange
            var user = new UserAddViewModel
            {
                Password = "teste123",
                Username = "garnica",
                Name = "Paulo Garnica",
                Email = "paulo.garnica@gmail.com"
            };

            var repository = await CreateRepositoryAsync();
            var personRepository = await CreatePersonRepositoryAsync();
            var personService = new PersonService(personRepository, repository);
            var userService = new UserService(repository, personService);
            await userService.Add(user);

            //Act
            var userRetorno = await userService.GetByUserName(user.Username);

            //Assert
            Assert.NotNull(userRetorno);
            Assert.Equal(user.Username, userRetorno.UserName);

        }

        [Fact]
        public async void PersonService_ValidateUser()
        {
            //Arrange
            var user = new UserAddViewModel
            {
                Password = "teste123",
                Username = "garnica",
                Name = "Paulo Garnica",
                Email = "paulo.garnica@gmail.com"
            };

            var repository = await CreateRepositoryAsync();
            var personRepository = await CreatePersonRepositoryAsync();
            var personService = new PersonService(personRepository, repository);
            var userService = new UserService(repository, personService);
            await userService.Add(user);

            //Act
            var userLogado = await userService.ValidateUser(user.Username, user.Password);

            //Assert
            Assert.NotNull(userLogado);
            Assert.Equal(user.Username, userLogado.UserName);

        }

        [Fact]
        public async void PersonService_Update()
        {
            //Arrange
            var user = new UserAddViewModel
            {
                Password = "teste123",
                Username = "garnica",
                Name = "Paulo Garnica",
                Email = "paulo.garnica@gmail.com"
            };

            var repository = await CreateRepositoryAsync();
            var personRepository = await CreatePersonRepositoryAsync();
            var personService = new PersonService(personRepository, repository);
            var userService = new UserService(repository, personService);
            await userService.Add(user);
            var userAntes = await userService.GetByUserName(user.Username);

            var userUpdate = new UserUpdateViewModel
            {
                Id = 1,
                Username = "garnica",
                Name = "Paulo Garnica",
                Email = "paulo.garnica@gmail.com",
                Active = true 
            };

            //Act
            var userLogado = await userService.Update(userUpdate);
            var userDepois = await userService.GetByUserName(user.Username);

            //Assert
            Assert.NotNull(userLogado);
            Assert.Equal(userDepois.UserName, userAntes.UserName);
        }

        [Fact]
        public async void PersonService_Remove()
        {
            //Arrange
            var user = new UserAddViewModel
            {
                Password = "teste123",
                Username = "garnica",
                Name = "Paulo Garnica",
                Email = "paulo.garnica@gmail.com"
            };

            var repository = await CreateRepositoryAsync();
            var personRepository = await CreatePersonRepositoryAsync();
            var personService = new PersonService(personRepository, repository);
            var userService = new UserService(repository, personService);
            await userService.Add(user);

            //Act
            var userLogado = await userService.Delete(new UserDeleteViewModel { Id = 1});
            var userDepois = await userService.GetByUserName(user.Username);

            //Assert
            Assert.NotNull(userLogado);
            Assert.Null(userDepois);
        }

        private async Task<UserRepository> CreateRepositoryAsync(bool populated = false)
        {
            ClinicaContext context = new ClinicaContext(_options);

            if (populated)
            {
                await PopulateDataAsync(context);
            }
            return new UserRepository(context);
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
