using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.API.Controllers;
using Web.Core.Models;
using Web.Core.Models.Dto;
using Web.Core.Services.Interfaces;
using Xunit;

namespace XUnitTestContacts
{
    public class UnitTestContacts
    {
        private readonly IContacts serviceContacts;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        public UnitTestContacts(IContacts servicio, IConfiguration config, ApplicationDbContext context) {
            serviceContacts = servicio;
            _configuration = config;
            _context = context;
        }
        [Fact]
        public async Task TestGetContactsAsync()
        {
            //Arrange
            var mockRepo = new Mock<IContacts>();
            mockRepo.Setup(repo => repo.GetAllModel())
                .ReturnsAsync(GetTestContactos());
            var controller = new ContactsController(mockRepo.Object);

            // Act
            var result = await controller.GetMock();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<ContactoDTO>>(
                viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());
        }


        private List<Contacts> GetTestContactos()
        {
            var contacts = new List<Contacts>();
            contacts.Add(new Contacts()
            {
                Company = "Compania",
                Email = "email1@dominio.com",
                FirstName = "Nombre",
                LastName = "Tarlao",
                PhoneNumber = "12312321",
                Id = 1
            });
            contacts.Add(new Contacts()
            {
                Company = "ase",
                Email = "email2@dominio.com",
                FirstName = "Nombre2",
                LastName = "Tarlao 2",
                PhoneNumber = "12312321",
                Id = 2
            });
            return contacts;
        }

    }
}
