using ClinicaHumaita.Data.Context;
using ClinicaHumaita.Data.Models;
using ClinicaHumaita.Data.Repository;
using ClinicaHumaita.Services;
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
        public async void PersonService_Create()
        {
            //Arrange
            var person = new Person
            {
                id = 10,
                name = "Paulo Garnica",
                email = "paulo.garnica@gmail.com"
            };

            User user = new User
            {
                Person = person,
                PersonId = person.id.Value,
                Active = true,
                Creation_Date = DateTime.Now,
                Last_login = null,
                Password = person.name,
                UserName = person.email
            };

            var repository = await CreateRepositoryAsync();
            var userService = new UserService(repository);

            //Act
            await userService.Create(user);

            var userRetorno = await userService.GetByUserName(user.UserName);

            //Assert
            Assert.NotNull(userRetorno);
            Assert.Equal(user.Id, userRetorno.Id);
            Assert.Equal(user.UserName, userRetorno.UserName);
           
        }

        [Fact]
        public async void PersonService_GetByUserName()
        {
            //Arrange
            var person = new Person
            {
                id = 10,
                name = "Paulo Garnica",
                email = "paulo.garnica@gmail.com"
            };

            User user = new User
            {
                Person = person,
                PersonId = person.id.Value,
                Active = true,
                Creation_Date = DateTime.Now,
                Last_login = null,
                Password = person.name,
                UserName = person.email
            };

            var repository = await CreateRepositoryAsync();
            var userService = new UserService(repository);
            await userService.Create(user);

            //Act
            var userRetorno = await userService.GetByUserName(user.UserName);

            //Assert
            Assert.NotNull(userRetorno);
            Assert.Equal(user.UserName, userRetorno.UserName);

        }

        [Fact]
        public async void PersonService_Login()
        {
            //Arrange
            var person = new Person
            {
                id = 10,
                name = "Paulo Garnica",
                email = "paulo.garnica@gmail.com"
            };

            User user = new User
            {
                Person = person,
                PersonId = person.id.Value,
                Active = true,
                Creation_Date = DateTime.Now,
                Last_login = null,
                Password = person.name,
                UserName = person.email
            };

            var repository = await CreateRepositoryAsync();
            var userService = new UserService(repository);
            await userService.Create(user);

            //Act
            var userLogado = await userService.ValidateUser(user.UserName, user.Person.name);

            //Assert
            Assert.NotNull(userLogado);
            Assert.Equal(user.UserName, userLogado.UserName);

        }

        [Fact]
        public async void PersonService_Edit()
        {
            //Arrange
            var person = new Person
            {
                id = 10,
                name = "Paulo Garnica",
                email = "paulo.garnica@gmail.com"
            };

            User user = new User
            {
                Person = person,
                PersonId = person.id.Value,
                Active = true,
                Creation_Date = DateTime.Now,
                Last_login = null,
                Password = person.name,
                UserName = person.email
            };

            var repository = await CreateRepositoryAsync();
            var userService = new UserService(repository);
            await userService.Create(user);
            var userAntes = await userService.GetByUserName(user.UserName);

            user.UserName = "garnica_username";

            

            //Act
            var userLogado = await userService.Edit(user);
            var userDepois = await userService.GetByUserName(user.UserName);

            //Assert
            Assert.NotNull(userLogado);
            Assert.Equal(userDepois.UserName, userAntes.UserName);
        }

        [Fact]
        public async void PersonService_Remove()
        {
            //Arrange
            var person = new Person
            {
                id = 10,
                name = "Paulo Garnica",
                email = "paulo.garnica@gmail.com"
            };

            User user = new User
            {
                Person = person,
                PersonId = person.id.Value,
                Active = true,
                Creation_Date = DateTime.Now,
                Last_login = null,
                Password = person.name,
                UserName = person.email
            };

            var repository = await CreateRepositoryAsync();
            var userService = new UserService(repository);
            await userService.Create(user);

            //Act
            var userLogado = await userService.Remove(user);
            var userDepois = await userService.GetByUserName(user.UserName);

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
