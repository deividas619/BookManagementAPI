using Microsoft.AspNetCore.Mvc;
using Moq;
using BookManagementAPI.Controllers;
using BookManagementAPI.DTOs;
using BookManagementAPI.Services;

namespace Tests
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly Mock<IJwtService> _jwtService;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _mockUserService = new Mock<IUserService>();
            _jwtService = new Mock<IJwtService>();
            _controller = new UserController(_mockUserService.Object, _jwtService.Object);
        }

        [Fact]
        public void Login_WithValidCredentials_ReturnsIsSuccessTrue()
        {
            // Arrange
            var userDto = new UserDto { Username = "test", Password = "tt" };
            var responseDto = new ResponseDto(isSuccess: true, message: "User logged in");
            _mockUserService.Setup(service => service.Login(userDto.Username, userDto.Password)).Returns(responseDto);

            // Act
            var result = _controller.Login(userDto.Username, userDto.Password);

            // Assert
            var actionResult = Assert.IsType<ActionResult<ResponseDto>>(result);
            var okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.Equal(200, okObjectResult.StatusCode);
        }

        [Fact]
        public void Login_WithInvalidCredentials_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var userDto = new UserDto { Username = "testuser", Password = "testpassword" };
            var responseDto = new ResponseDto(isSuccess: false, message: "Username or password does not match");
            _mockUserService.Setup(service => service.Login(userDto.Username, userDto.Password)).Returns(responseDto);

            // Act
            var result = _controller.Login(userDto.Username, userDto.Password);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.NotNull(badRequestObjectResult.Value);
            var resultMessage = Assert.IsType<string>(badRequestObjectResult.Value);
            Assert.Equal("Username or password does not match", resultMessage);
        }

        [Fact]
        public void Signup_WithValidUser_ReturnsIsSuccessTrue()
        {
            // Arrange
            var userDto = new UserDto { Username = "newuser", Password = "newpassword" };
            var responseDto = new ResponseDto(isSuccess: true);
            _mockUserService.Setup(service => service.Signup(userDto.Username, userDto.Password)).Returns(responseDto);

            // Act
            var result = _controller.Signup(userDto);

            // Assert
            var actionResult = Assert.IsType<ActionResult<ResponseDto>>(result);
            Assert.NotNull(actionResult.Value);
            var resultDto = Assert.IsType<ResponseDto>(actionResult.Value);
            Assert.True(resultDto.IsSuccess);
        }

        [Fact]
        public void Signup_WithExistingUser_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var userDto = new UserDto { Username = "existinguser", Password = "existingpassword" };
            var responseDto = new ResponseDto(isSuccess: false, message: "User already exists");
            _mockUserService.Setup(service => service.Signup(userDto.Username, userDto.Password)).Returns(responseDto);

            // Act
            var result = _controller.Signup(userDto);

            // Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            var resultMessage = Assert.IsType<string>(badRequestObjectResult.Value);
            Assert.Equal("User already exists", resultMessage);
        }
    }
}
