using ClinicaHumaita.Controllers;
using ClinicaHumaita.Models;
using ClinicaHumaita.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Graph;
using Person = ClinicaHumaita.Models.Person;
using ClinicaHumaita.Services;
using System.Linq;

namespace ClinicaHumaitaTests
{
    public class PersonServiceTests
    {
        private readonly Mock<IPersonServices> _mockPerson;
        private readonly PersonController _personController;

        public PersonServiceTests()
        {
            _mockPerson = new Mock<IPersonServices>();
            _personController = new PersonController(_mockPerson.Object);
        }

        [Theory]
        [InlineData("Paulo Garnica","paulo.garnica@gail.com")]
        public async Task Add_Person(string name, string email)
        {
            //arrange
            var options = new DbContextOptionsBuilder<ClinicaContext>().UseInMemoryDatabase(databaseName: "TestNewListDb").Options;

            // Set up a context (connection to the "DB") for writing
            using (var context = new ClinicaContext(options))
            {
                // 1. Arrange
                var rl = new Person
                {
                    name = name,
                    email = email
                };

                // 2. Act 
                var rls = new PersonService(context);
                await rls.Create(rl);
            }

            using (var context = new ClinicaContext(options))
            {
                var rls = new PersonService(context);
                var result = await rls.Get();

                // 3. Assert
                Assert.NotEmpty(result);
                Assert.NotEmpty(result.Last().name);
                Assert.Equal(name, result.Last().name);
                Assert.Equal(email, result.Last().email);
            }
        }
        [Theory]
        [InlineData(1)]
        public async Task GetPersonById(int id)
        {
            //arrange
            var options = new DbContextOptionsBuilder<ClinicaContext>().UseInMemoryDatabase(databaseName: "TestNewListDb").Options;


            // Set up a context (connection to the "DB") for writing
            using (var context = new ClinicaContext(options))
            {
                // 1. Arrange
                var rl = new Person
                {
                    name = "Paulo",
                    email = "email"
                };

                // 2. Act 
                var rls = new PersonService(context);
                await rls.Create(rl);
            }

            using (var context = new ClinicaContext(options))
            {
                var rls = new PersonService(context);
                var result = await rls.GetById(id);

                // 3. Assert
                Assert.IsType<Person>(result);
                Assert.Equal(id, result.id);
            }

        }


    }
}
