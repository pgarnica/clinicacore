using ClinicaHumaita.Interfaces;
using ClinicaHumaita.Controllers;
using ClinicaHumaita.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ClinicaHumaitaTests
{
    public class PersonControllerTests
    {
        private readonly Mock<IPersonServices> _mockPerson;
        private readonly PersonController _personController;

        public PersonControllerTests()
        {
            _mockPerson = new Mock<IPersonServices>();
            _personController = new PersonController(_mockPerson.Object);
        }

        [Fact]
        public async void Index_PersonController()
        {
            var result = await _personController.Index();
            Assert.IsType<ViewResult>(result);
        }
    }
}
