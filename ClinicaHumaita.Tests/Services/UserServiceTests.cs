using AutoFixture;
using ClinicaHumaita.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace ClinicaHumaita.Tests.Services
{
    public class UserServiceTests
    {
        private DbContextOptions<ClinicaContext> _options;
        private DbContextOptions<ClinicaContext> _optionsInMemory;
        private IFixture _fixture;
        IConfiguration Configuration { get; set; }
        public UserServiceTests()
        {
            _options = new DbContextOptionsBuilder<ClinicaContext>().UseInMemoryDatabase(databaseName: "TestNewListDb").Options;
            var builder = new ConfigurationBuilder().AddUserSecrets<PersonServiceTests>();
            Configuration = builder.Build();
            _options = new DbContextOptionsBuilder<ClinicaContext>().UseSqlServer(Configuration["ConnectionStrings:Clinica"]).Options;
            _optionsInMemory = new DbContextOptionsBuilder<ClinicaContext>().UseInMemoryDatabase(databaseName: "TestNewListDb").Options;
            _fixture = new Fixture();

        }
    }
}
