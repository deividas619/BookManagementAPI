using Moq;
using BookManagementAPI.Models;
using BookManagementAPI.Services;
using BookManagementAPI.Interfaces;

namespace Tests
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _userService = new UserService(_mockUserRepository.Object);
        }

        [Fact]
        public void Login_WithValidCredentials_ReturnsSuccessResponse()
        {
            // Arrange
            var username = "testuser";
            var password = "testpassword";

            _userService.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = username,
                Password = passwordHash,
                PasswordSalt = passwordSalt,
                Role = UserRole.Regular
            };

            _mockUserRepository.Setup(repo => repo.GetUser(username)).Returns(user);

            // Act
            var response = _userService.Login(username, password);

            // Assert
            Assert.True(response.IsSuccess);
            Assert.Equal("User logged in!", response.Message);
        }

        [Fact]
        public void Login_WithInvalidCredentials_ReturnsErrorResponse()
        {
            // Arrange
            var username = "testuser";
            var password = "testpassword";

            _mockUserRepository.Setup(repo => repo.GetUser(username)).Returns((User)null);

            // Act
            var response = _userService.Login(username, password);

            // Assert
            Assert.False(response.IsSuccess);
            Assert.Equal("Username does not exist!", response.Message);
        }

        [Fact]
        public void Signup_WithNewUsername_ReturnsSuccessResponse()
        {
            // Arrange
            var username = "newuser";
            var password = "newpassword";

            _mockUserRepository.Setup(repo => repo.GetUser(username)).Returns((User)null);

            // Act
            var response = _userService.Signup(username, password);

            // Assert
            Assert.True(response.IsSuccess);
            Assert.Equal("User created!", response.Message);
            _mockUserRepository.Verify(repo => repo.SaveNewUser(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public void Signup_WithExistingUsername_ReturnsErrorResponse()
        {
            // Arrange
            var existingUsername = "existinguser";
            var password = "existingpassword";
            var existingUser = new User
            {
                Id = Guid.NewGuid(),
                Username = existingUsername,
                Password = new byte[] { 120, 227, 65, 14, 24 },
                PasswordSalt = new byte[] { 68, 13, 2, 215, 33 }
            };

            _mockUserRepository.Setup(repo => repo.GetUser(existingUsername)).Returns(existingUser);

            // Act
            var response = _userService.Signup(existingUsername, password);

            // Assert
            Assert.False(response.IsSuccess);
            Assert.Equal("User already exists!", response.Message);
            _mockUserRepository.Verify(repo => repo.SaveNewUser(It.IsAny<User>()), Times.Never);
        }
    }
}
