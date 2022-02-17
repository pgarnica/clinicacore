using AutoFixture;
using AutoMapper;
using ClinicaHumaita.Business.Interfaces;
using ClinicaHumaita.Data.Context;
using ClinicaHumaita.Data.Models;
using ClinicaHumaita.Data.Repository;
using ClinicaHumaita.Services;
using ClinicaHumaita.Shared.ViewModels;
using ClinicaHumaita.Tests.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Net;
using System.Net.Mail;
using Xunit;

namespace ClinicaHumaita.Tests.Services
{
    public class PersonServiceTests  
    {
        private DbContextOptions<ClinicaContext> _options;
        private DbContextOptions<ClinicaContext> _optionsInMemory;
        private IFixture _fixture;
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;
        IConfiguration Configuration { get; set; }

        public PersonServiceTests()
        {
            var builder = new ConfigurationBuilder().AddUserSecrets<PersonServiceTests>();
            Configuration = builder.Build();
            _options = new DbContextOptionsBuilder<ClinicaContext>().UseSqlServer(Configuration["ConnectionStrings:Clinica"]).Options;
            _optionsInMemory = new DbContextOptionsBuilder<ClinicaContext>().UseInMemoryDatabase(databaseName: "TestNewListDb").Options;
            _fixture = new Fixture();
            _notificationService = new NotificationService();

            var autoMapperConfiguration = new AutoMapperConfiguration();
            _mapper = autoMapperConfiguration.createAutoMapperConfig().CreateMapper();
        }

        [Fact]
        public async void PersonService_GetById_PersonFound()
        {
            //Arrange
            var _personService = personServiceInitialization();
            _fixture.Customize<PersonAddViewModel>(c => c.With(x => x.Email, _fixture.Create<MailAddress>().Address));
            var personAddViewModel = _fixture.Create<PersonAddViewModel>();
            var person = await _personService.Add(personAddViewModel);

            //Act
            var personGetById = await _personService.GetById(person.id);

            //Assert
            Assert.NotNull(personGetById);
            Assert.Equal(personAddViewModel.Name, personGetById.name);
            Assert.Equal(personAddViewModel.Email, personGetById.email);
        }

        [Fact]
        public async void PersonService_GetById_PersonNotFound()
        {
            //Arrange
            var _personService = personServiceInitialization();

            //Act
            var personGetById = await _personService.GetById(999);

            //Assert
            Assert.Null(personGetById);
        }

        [Fact]
        public async void PersonService_Add_NameIsRequired()
        {
            //Arrange
            var _personService = personServiceInitialization();
            _fixture.Customize<PersonAddViewModel>(c => c.With(x => x.Email, _fixture.Create<MailAddress>().Address));
            var personAddViewModel = _fixture.Create<PersonAddViewModel>();
            personAddViewModel.Name = null;

            //Act
            var person = await _personService.Add(personAddViewModel);
           
            var errors = _notificationService.getErrors();

            //Assert
            Assert.Null(person);
            Assert.Equal("Name field must be provided.", errors.Message);
            Assert.Equal(HttpStatusCode.BadRequest, errors.StatusCode);
        }



        private PersonService personServiceInitialization(bool inMemory = false)
        {
            ClinicaContext context = new ClinicaContext(inMemory? _optionsInMemory :_options);
            var personRepository = new PersonRepository(context);
            var userRepository = new UserRepository(context);
            var rabbitMqService = new Mock<IRabbitMQService>();
            return new PersonService(personRepository, userRepository, _mapper, rabbitMqService.Object, _notificationService);
        }
    }
}
