using BankingApi.Controllers;
using BankingApi.Data.Services;
using BankingApi.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BankingApi
{
    public class InstitutionsControllerTests
    {
        [Fact]
        public void Should_get_institution_list()
        {
            //
            // Arrange
            var testInstitution = new InstitutionDto
            {
                Id = Guid.NewGuid(),
                Name = "Test Institutions",
                Phone = "1-800-555-0000",
                Email = "test@test.com",
                Website = "http://test.com",
                CreatedAt = DateTime.UtcNow
            };

            var mockInstitutionService = new Mock<IInstitutionService>();

            mockInstitutionService
                .Setup(svc => svc.GetAll())
                .Returns(new List<InstitutionDto> { testInstitution });

            var institutionsController = new InstitutionsController(mockInstitutionService.Object);

            //
            // Act
            var result = institutionsController.Get();

            //
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<InstitutionDto>>(okResult.Value);
            var institutionResult = model.FirstOrDefault();
            
            Assert.Equal(testInstitution.Id, institutionResult.Id);
        }
    }
}
