using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ShipeX2.Application.DTOs;
using ShipeX2.Application.Interfaces;
using ShipeX2.Web.Controllers;
using Xunit;

namespace ShipeX2.Tests.Controllers
{
    public class CarrierControllerTests
    {
        [Fact]
        public async Task EditCarrierApi_ShouldReturn200Ok_WithCarrierDetails_WhenIdIsValid ()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<CarrierController>>();
            var mockCarrierServices = new Mock<ICarrierListServices>();
            var controller = new CarrierController(mockLogger.Object, mockCarrierServices.Object);

            long validCarrierId = 1;
            var expectedModel = new ModelShipCarrier
            {
                CarrierId = (int) validCarrierId,
                CarrierName = "Test Carrier",
                DefaultAccountNo = "12345" // Fix: Set required property DefaultAccountNo
            };
            mockCarrierServices.Setup(s => s.GetCarrierByIdAsync(validCarrierId)).ReturnsAsync(expectedModel);

            // Act
            var result = await controller.EditCarrierApi(validCarrierId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(expectedModel, viewResult.Model);
            Assert.Equal(200, (int) viewResult.StatusCode);
        }
    }
}