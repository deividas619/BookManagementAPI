using BookManagementAPI.Controllers;
using BookManagementAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using Moq;
using BookManagementAPI.Interfaces;

namespace Tests
{
    public class AdminControllerTests
    {
        [Fact]
        public async Task SetAdmin_Returns_OkResult_With_ResponseDto()
        {
            // Arrange
            var username = "testUser";
            var newRole = "Admin";
            var mockAdminService = new Mock<IAdminService>();
            mockAdminService.Setup(service => service.SetAdminAndChangeRoleAsync(username, newRole))
                            .ReturnsAsync(new ResponseDto(true, "User role changed to Admin successfully"));
            var controller = new AdminController(mockAdminService.Object);

            // Act
            var result = await controller.SetAdmin(username, newRole);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ResponseDto>(okResult.Value);
            Assert.True(response.IsSuccess);
            Assert.Equal("User role changed to Admin successfully", response.Message);
        }

        [Fact]
        public async Task RevokeAdmin_Returns_OkResult_With_ResponseDto()
        {
            // Arrange
            var username = "testUser";
            var newRole = "Regular";
            var mockAdminService = new Mock<IAdminService>();
            mockAdminService.Setup(service => service.RevokeAdminAndChangeRoleAsync(username, newRole))
                            .ReturnsAsync(new ResponseDto(true, "User role changed to Regular successfully"));
            var controller = new AdminController(mockAdminService.Object);

            // Act
            var result = await controller.RevokeAdmin(username, newRole);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ResponseDto>(okResult.Value);
            Assert.True(response.IsSuccess);
            Assert.Equal("User role changed to Regular successfully", response.Message);
        }
    }
}