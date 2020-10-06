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
        public async Task Create_Person(string name, string email)
        {
            //arrange
            //Cria uma database virtual para nao sujar a base com testes
            var options = new DbContextOptionsBuilder<ClinicaContext>().UseInMemoryDatabase(databaseName: "TestNewListDb").Options;

            // cria o context para acesso ao db, utilizado a base virtual
            using (var context = new ClinicaContext(options))
            {
                // 1. Arrange
                //cria uma nova pessoa
                var rl = new Person
                {
                    name = name,
                    email = email
                };

                // 2. Act 
                //instancia o servico para ser utilizado com a base virtual
                var rls = new PersonService(context);
                await rls.Create(rl);
        
                //busca uma lista das pessoas inseridas
                var result = await rls.Get();

                // 3. Assert
                //verifica se os resultados estao corretos
                Assert.NotEmpty(result);
                Assert.NotEmpty(result.Last().name);
                Assert.Equal(name, result.Last().name);
                Assert.Equal(email, result.Last().email);
            }
        }

        [Theory]
        [InlineData("Paulo Garnica", "paulo.garnica@gail.com","Paulo Alterado","paulo.alterado@gmail.com")]
        public async Task Edit_Person(string name, string email,string new_name, string new_email)
        {
            //arrange
            //Cria uma database virtual para nao sujar a base com testes
            var options = new DbContextOptionsBuilder<ClinicaContext>().UseInMemoryDatabase(databaseName: "TestNewListDb").Options;

            // cria o context para acesso ao db, utilizado a base virtual
            using (var context = new ClinicaContext(options))
            {
                // 1. Arrange
                //cria uma nova pessoa
                var rl = new Person
                {
                    name = name,
                    email = email
                };

                // 2. Act 
                //instancia o servico para ser utilizado com a base virtual
                var rls = new PersonService(context);
                
                //adiciona uma nova pessoa
                await rls.Create(rl);

                //recupera a pessoa inserida
                var person = await rls.GetById(rl.id);

                //altera is dados da pessoa inserida
                person.name = new_name;
                person.email = new_email;
                //envia as alteracoes
                var result = await rls.Edit(person);

                // 3. Assert
                //verifica se os dados alterados estao corretos
                Assert.NotNull(result);
                Assert.Equal(new_name, result.name);
                Assert.Equal(new_email, result.email);
            }
        }

        [Theory]
        [InlineData("Paulo Garnica", "paulo.garnica@gail.com")]
        public async Task Remove_Person(string name, string email)
        {
            //arrange
            //Cria uma database virtual para nao sujar a base com testes
            var options = new DbContextOptionsBuilder<ClinicaContext>().UseInMemoryDatabase(databaseName: "TestNewListDb").Options;

            // cria o context para acesso ao db, utilizado a base virtual
            using (var context = new ClinicaContext(options))
            {
                // 1. Arrange
                //cria uma nova pessoa
                var rl = new Person
                {
                    name = name,
                    email = email
                };

                // 2. Act 
                //instancia o servico para ser utilizado com a base virtual
                var rls = new PersonService(context);

                //adiciona uma nova pessoa
                await rls.Create(rl);

                //recupera a pessoa inserida
                var person = await rls.GetById(rl.id);

                //envia as alteracoes
                var result = await rls.Remove(person);

                var buscaremovido = await rls.GetById(person.id);

                // 3. Assert
                //verifica se os dados alterados estao corretos
                Assert.NotNull(result);
                Assert.Null(buscaremovido);
                Assert.Equal(true, result);
            }
        }


        [Theory]
        [InlineData(1)]
        public async Task GetPersonById(int id)
        {
            //arrange
            //Cria uma database virtual para nao sujar a base com testes
            var options = new DbContextOptionsBuilder<ClinicaContext>().UseInMemoryDatabase(databaseName: "TestNewListDb").Options;


            // cria o context para acesso ao db, utilizado a base virtual
            using (var context = new ClinicaContext(options))
            {
                // 1. Arrange
                //Cria um novo objeto user com os parametros informados
                var rl = new Person
                {
                    name = "Paulo",
                    email = "email"
                };

                // 2. Act 
                //instancia o servico para ser utilizado com a base virtual
                var rls = new PersonService(context);

                //adiciona o novo User
                await rls.Create(rl);


                // busca a pessoa no banco pelo id
                var result = await rls.GetById(id);

                // 3. Assert
                //verifica se os resultados estao corretos
                Assert.IsType<Person>(result);
                Assert.Equal(id, result.id);
            }

        }


    }
}
