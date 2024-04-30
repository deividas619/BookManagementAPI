using BookManagementAPI.DTOs;
using BookManagementAPI.Models;
using BookManagementAPI.Services.Repositories;
using BookManagementAPI.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class AdminServicesTests
    {
        [Fact]
        public async Task CreateAdminAsync_Returns_SuccessResponseDto_When_Admin_Does_Not_Exist()
        {
            // Arrange
            var username = "testUser";
            var password = "testPassword";
            var adminRepositoryMock = new Mock<IAdminRepository>();
            adminRepositoryMock.Setup(repo => repo.GetAdminAsync(It.IsAny<string>())).ReturnsAsync((Admin)null);
            var userRepositoryMock = new Mock<IUserRepository>();
            var bookRepositoryMock = new Mock<IBookRepository>();
            var adminService = new AdminServices(adminRepositoryMock.Object, userRepositoryMock.Object, bookRepositoryMock.Object);

            // Act
            var result = await adminService.CreateAdminAsync(username, password);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Admin created successfully", result.Message);
        }

        [Fact]
        public async Task CreateAdminAsync_Returns_FailureResponseDto_When_Admin_Exists()
        {
            // Arrange
            var username = "testUser";
            var password = "testPassword";
            var adminRepositoryMock = new Mock<IAdminRepository>();
            adminRepositoryMock.Setup(repo => repo.GetAdminAsync(It.IsAny<string>())).ReturnsAsync(new Admin());
            var userRepositoryMock = new Mock<IUserRepository>();
            var bookRepositoryMock = new Mock<IBookRepository>();
            var adminService = new AdminServices(adminRepositoryMock.Object, userRepositoryMock.Object, bookRepositoryMock.Object);

            // Act
            var result = await adminService.CreateAdminAsync(username, password);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Admin already exists", result.Message);
        }

        [Fact]
        public async Task UpdateAdminAsync_Returns_SuccessResponseDto_When_Admin_Exists()
        {
            // Arrange
            var username = "testUser";
            var adminRepositoryMock = new Mock<IAdminRepository>();
            adminRepositoryMock.Setup(repo => repo.GetAdminAsync(It.IsAny<string>())).ReturnsAsync(new Admin());
            adminRepositoryMock.Setup(repo => repo.UpdateAdminAsync(It.IsAny<Admin>())).Returns(Task.CompletedTask);
            var userRepositoryMock = new Mock<IUserRepository>();
            var bookRepositoryMock = new Mock<IBookRepository>();
            var adminService = new AdminServices(adminRepositoryMock.Object, userRepositoryMock.Object, bookRepositoryMock.Object);

            // Act
            var result = await adminService.UpdateAdminAsync(username);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Admin updated successfully", result.Message);
        }

        [Fact]
        public async Task UpdateAdminAsync_Returns_FailureResponseDto_When_Admin_Does_Not_Exist()
        {
            // Arrange
            var username = "testUser";
            var adminRepositoryMock = new Mock<IAdminRepository>();
            adminRepositoryMock.Setup(repo => repo.GetAdminAsync(It.IsAny<string>())).ReturnsAsync((Admin)null);
            var userRepositoryMock = new Mock<IUserRepository>();
            var bookRepositoryMock = new Mock<IBookRepository>();
            var adminService = new AdminServices(adminRepositoryMock.Object, userRepositoryMock.Object, bookRepositoryMock.Object);

            // Act
            var result = await adminService.UpdateAdminAsync(username);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Admin not found", result.Message);
        }

        [Fact]
        public async Task DeleteAdminAsync_Returns_SuccessResponseDto_When_Admin_Exists()
        {
            // Arrange
            var username = "testUser";
            var adminRepositoryMock = new Mock<IAdminRepository>();
            adminRepositoryMock.Setup(repo => repo.GetAdminAsync(It.IsAny<string>())).ReturnsAsync(new Admin());
            adminRepositoryMock.Setup(repo => repo.DeleteAdminAsync(It.IsAny<Admin>())).Returns(Task.CompletedTask);
            var userRepositoryMock = new Mock<IUserRepository>();
            var bookRepositoryMock = new Mock<IBookRepository>();
            var adminService = new AdminServices(adminRepositoryMock.Object, userRepositoryMock.Object, bookRepositoryMock.Object);

            // Act
            var result = await adminService.DeleteAdminAsync(username);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Admin deleted successfully", result.Message);
        }

        [Fact]
        public async Task DeleteAdminAsync_Returns_FailureResponseDto_When_Admin_Does_Not_Exist()
        {
            // Arrange
            var username = "testUser";
            var adminRepositoryMock = new Mock<IAdminRepository>();
            adminRepositoryMock.Setup(repo => repo.GetAdminAsync(It.IsAny<string>())).ReturnsAsync((Admin)null);
            var userRepositoryMock = new Mock<IUserRepository>();
            var bookRepositoryMock = new Mock<IBookRepository>();
            var adminService = new AdminServices(adminRepositoryMock.Object, userRepositoryMock.Object, bookRepositoryMock.Object);

            // Act
            var result = await adminService.DeleteAdminAsync(username);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Admin not found", result.Message);
        }

        [Fact]
        public async Task DeleteBookAsync_Returns_SuccessResponseDto_When_Book_Exists()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var bookRepositoryMock = new Mock<IBookRepository>();
            bookRepositoryMock.Setup(repo => repo.GetBookById(It.IsAny<Guid>())).ReturnsAsync(new Book());
            bookRepositoryMock.Setup(repo => repo.RemoveBookById(It.IsAny<Guid>()));
            var adminRepositoryMock = new Mock<IAdminRepository>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var adminService = new AdminServices(adminRepositoryMock.Object, userRepositoryMock.Object, bookRepositoryMock.Object);

            // Act
            var result = await adminService.DeleteBookAsync(bookId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Book deleted successfully", result.Message);
        }

        [Fact]
        public async Task DeleteBookAsync_Returns_FailureResponseDto_When_Book_Does_Not_Exist()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var bookRepositoryMock = new Mock<IBookRepository>();
            bookRepositoryMock.Setup(repo => repo.GetBookById(It.IsAny<Guid>())).ReturnsAsync((Book)null);
            var adminRepositoryMock = new Mock<IAdminRepository>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var adminService = new AdminServices(adminRepositoryMock.Object, userRepositoryMock.Object, bookRepositoryMock.Object);

            // Act
            var result = await adminService.DeleteBookAsync(bookId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Book not found", result.Message);
        }

        [Fact]
        public async Task SetAdminAndChangeRoleAsync_Returns_SuccessResponseDto_When_User_Exists()
        {
            // Arrange
            var username = "testUser";
            var newRole = "Admin";
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repo => repo.GetUserAsync(It.IsAny<string>())).ReturnsAsync(new User { Username = username });
            userRepositoryMock.Setup(repo => repo.SaveChangedUser(It.IsAny<User>())).Callback<User>(user => { });
            var adminRepositoryMock = new Mock<IAdminRepository>();
            var bookRepositoryMock = new Mock<IBookRepository>();
            var adminService = new AdminServices(adminRepositoryMock.Object, userRepositoryMock.Object, bookRepositoryMock.Object);

            // Act
            var result = await adminService.SetAdminAndChangeRoleAsync(username, newRole);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("User role changed to Admin successfully", result.Message);
        }

        [Fact]
        public async Task SetAdminAndChangeRoleAsync_Returns_FailureResponseDto_When_User_Does_Not_Exist()
        {
            // Arrange
            var username = "testUser";
            var newRole = "Admin";
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repo => repo.GetUserAsync(It.IsAny<string>())).ReturnsAsync((User)null);
            var adminRepositoryMock = new Mock<IAdminRepository>();
            var bookRepositoryMock = new Mock<IBookRepository>();
            var adminService = new AdminServices(adminRepositoryMock.Object, userRepositoryMock.Object, bookRepositoryMock.Object);

            // Act
            var result = await adminService.SetAdminAndChangeRoleAsync(username, newRole);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("User not found", result.Message);
        }

        [Fact]
        public async Task RevokeAdminAndChangeRoleAsync_Returns_SuccessResponseDto_When_User_Exists()
        {
            // Arrange
            var username = "testUser";
            var newRole = "Regular";
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repo => repo.GetUserAsync(It.IsAny<string>())).ReturnsAsync(new User { Username = username });
            userRepositoryMock.Setup(repo => repo.SaveChangedUser(It.IsAny<User>())).Callback<User>(user => { });
            var adminRepositoryMock = new Mock<IAdminRepository>();
            var bookRepositoryMock = new Mock<IBookRepository>();
            var adminService = new AdminServices(adminRepositoryMock.Object, userRepositoryMock.Object, bookRepositoryMock.Object);

            // Act
            var result = await adminService.RevokeAdminAndChangeRoleAsync(username, newRole);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("User role changed to Regular successfully", result.Message);
        }

        [Fact]
        public async Task RevokeAdminAndChangeRoleAsync_Returns_FailureResponseDto_When_User_Does_Not_Exist()
        {
            // Arrange
            var username = "testUser";
            var newRole = "Regular";
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repo => repo.GetUserAsync(It.IsAny<string>())).ReturnsAsync((User)null);
            var adminRepositoryMock = new Mock<IAdminRepository>();
            var bookRepositoryMock = new Mock<IBookRepository>();
            var adminService = new AdminServices(adminRepositoryMock.Object, userRepositoryMock.Object, bookRepositoryMock.Object);

            // Act
            var result = await adminService.RevokeAdminAndChangeRoleAsync(username, newRole);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("User not found", result.Message);
        }

        [Fact]
        public async Task AddBookAsync_Returns_SuccessResponseDto_When_Book_Added_Successfully()
        {
            // Arrange
            var bookDto = new BookDto
            {
                Title = "Test Book",
                Author = "Test Author",
                Publication = new DateOnly(2022, 1, 1),
                Genre = new GenreDto() 
            };

            var bookRepositoryMock = new Mock<IBookRepository>();
            bookRepositoryMock.Setup(repo => repo.AddBook(It.IsAny<Book>())).Returns(Task.FromResult(new Book()));
            var adminRepositoryMock = new Mock<IAdminRepository>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var adminService = new AdminServices(adminRepositoryMock.Object, userRepositoryMock.Object, bookRepositoryMock.Object);

            // Act
            var result = await adminService.AddBookAsync(bookDto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Book added successfully", result.Message);
        }

        [Fact]
        public async Task AddBookAsync_Returns_FailureResponseDto_When_Book_Addition_Fails()
        {
            // Arrange
            var bookDto = new BookDto { Title = "Test Book", Author = "Test Author", Publication = DateOnly.FromDateTime(DateTime.UtcNow) };
            var bookRepositoryMock = new Mock<IBookRepository>();
            bookRepositoryMock.Setup(repo => repo.AddBook(It.IsAny<Book>())).Returns(Task.FromException<Book>(new Exception("Failed to add book")));
            var adminRepositoryMock = new Mock<IAdminRepository>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var adminService = new AdminServices(adminRepositoryMock.Object, userRepositoryMock.Object, bookRepositoryMock.Object);

            // Act
            var result = await adminService.AddBookAsync(bookDto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Failed to add book", result.Message);
        }
    }
}
