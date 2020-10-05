using ClinicaHumaita.Controllers;
using ClinicaHumaita.Interfaces;
using ClinicaHumaita.Models;
using ClinicaHumaita.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ClinicaHumaitaTests
{
    public class UsersServiceTest
    {
        private readonly Mock<IUsersServices> _mockUsers;
        private readonly UserController _userController;

        public UsersServiceTest()
        {
            _mockUsers = new Mock<IUsersServices>();
            _userController = new UserController(_mockUsers.Object);
        }

        [Theory]
        [InlineData("Paulo Garnica", "paulo.garnica@gail.com", "paulogarnica","senha123")]
        public async Task Create_User(string name, string email,string username,string password)
        {
            //arrange
            //Cria uma database virtual para nao sujar a base com testes
            var options = new DbContextOptionsBuilder<ClinicaContext>().UseInMemoryDatabase(databaseName: "TestNewListDb").Options;

            // cria o context para acesso ao db, utilizado a base virtual
            using (var context = new ClinicaContext(options))
            {
                // 1. Arrange
                //Cria um novo objeto user com os parametros informados
                var rl = new Users
                {
                    UserName = username,
                    Password = password,
                    Person = new Person { name = name, email = email }
                };

                // 2. Act 
                //instancia o servico para ser utilizado com a base virtual
                var rls = new UsersServices(context);
                //adiciona o novo User
                await rls.Create(rl);
                //busca o novo user pelo username
                var result = await rls.GetByUserName(username);

                // 3. Assert
                //testa se o retorno nao eh nulo
                Assert.NotNull(result);
                //testa se os mesmo parametros informados sao recebidos
                Assert.Equal(username, result.UserName);
                Assert.Equal(name, result.Person.name);
                Assert.Equal(email, result.Person.email);
                Assert.Equal(rls.MD5Hash(password), result.Password);
            }
        }

        [Theory]
        [InlineData("Paulo Garnica", "paulo.garnica@gail.com", "paulogarnica", "senha123")]
        public async Task Login_User(string name, string email, string username, string password)
        {
            //arrange
            //Cria uma database virtual para nao sujar a base com testes
            var options = new DbContextOptionsBuilder<ClinicaContext>().UseInMemoryDatabase(databaseName: "TestNewListDb").Options;

            // cria o context para acesso ao db, utilizado a base virtual
            using (var context = new ClinicaContext(options))
            {
                // 1. Arrange
                //Cria um novo objeto user com os parametros informados
                var rl = new Users
                {
                    UserName = username,
                    Password = password,
                    Person = new Person { name = name, email = email }
                };

                // 2. Act 
                //instancia o servico para ser utilizado com a base virtual
                var rls = new UsersServices(context);
                //adiciona o novo User
                await rls.Create(rl);
                //realiza o login do usuario com os dados informados por parametros
                var result = await rls.Login(username, password);

                // 3. Assert
                //testa se o retorno nao eh nulo
                Assert.NotNull(result);
                //testa se os mesmo parametros informados sao recebidos
                Assert.Equal(username, result.UserName);
                Assert.Equal(name, result.Person.name);
                Assert.Equal(email, result.Person.email);
                Assert.Equal(rls.MD5Hash(password), result.Password);
            }
        }

        [Theory]
        [InlineData("Paulo Garnica", "paulo.garnica@gail.com", "paulogarnica", "senha123","NomeNovo","email@novo.com.br","usuariogarnica","123senha")]
        public async Task Edit_User(string name, string email, string username, string password,string new_name,string new_email,string new_username,string new_password)
        {
            //arrange
            //Cria uma database virtual para nao sujar a base com testes
            var options = new DbContextOptionsBuilder<ClinicaContext>().UseInMemoryDatabase(databaseName: "TestNewListDb").Options;

            // cria o context para acesso ao db, utilizado a base virtual
            using (var context = new ClinicaContext(options))
            {
                // 1. Arrange
                //Cria um novo objeto user com os parametros informados
                var rl = new Users
                {
                    UserName = username,
                    Password = password,
                    Person = new Person { name = name, email = email }
                };

                // 2. Act 
                //instancia o servico para ser utilizado com a base virtual
                var rls = new UsersServices(context);

                //adiciona o novo User
                await rls.Create(rl);

                //busca o usuario no banco pelo username
                rl = rls.GetByUserName(username).Result;

                //altera os valores para novos valores
                rl.Person.name = new_name;
                rl.Person.email = new_email;
                rl.UserName = new_username;
                rl.Password = new_password;

                //adiciona o novo User
                await rls.Edit(rl);
            
               //busca o novo usuario
                var result = await rls.GetByUserName(new_username);

                // 3. Assert
                //testa se o retorno nao eh nulo
                Assert.NotNull(result);
                //testa se os mesmo parametros informados sao recebidos
                Assert.Equal(new_username, result.UserName);
                Assert.Equal(new_name, result.Person.name);
                Assert.Equal(new_email, result.Person.email);
                Assert.Equal(rls.MD5Hash(new_password), result.Password);
            }
        }
    }
}
