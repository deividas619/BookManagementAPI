using Microsoft.EntityFrameworkCore;
using Moq;
using BookManagementAPI.Models;
using BookManagementAPI;
using BookManagementAPI.Repositories;

namespace Tests
{
    public class UserRepositoryTests
    {
        [Fact]
        public void GetUser_ExistingUsername_ReturnsUser()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = Guid.NewGuid(), Username = "existinguser", Password = new byte[0], PasswordSalt = new byte[0] }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<User>>();
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());

            var mockContext = new Mock<ApplicationDbContext>();
            mockContext.Setup(c => c.Users).Returns(mockDbSet.Object);

            var userRepository = new UserRepository(mockContext.Object);

            // Act
            var result = userRepository.GetUser("existinguser");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("existinguser", result.Username);
        }

        [Fact]
        public void GetUser_NonExistingUsername_ReturnsNull()
        {
            // Arrange
            var users = new List<User>().AsQueryable();

            var mockDbSet = new Mock<DbSet<User>>();
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());

            var mockContext = new Mock<ApplicationDbContext>();
            mockContext.Setup(c => c.Users).Returns(mockDbSet.Object);

            var userRepository = new UserRepository(mockContext.Object);

            // Act
            var result = userRepository.GetUser("nonexistinguser");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void SaveUser_NewUser_SavesUser()
        {
            // Arrange
            var newUser = new User { Id = Guid.NewGuid(), Username = "newuser", Password = new byte[0], PasswordSalt = new byte[0] };

            var mockDbSet = new Mock<DbSet<User>>();
            var mockContext = new Mock<ApplicationDbContext>();
            mockContext.Setup(c => c.Users).Returns(mockDbSet.Object);

            var userRepository = new UserRepository(mockContext.Object);

            // Act
            userRepository.SaveNewUser(newUser);

            // Assert
            mockDbSet.Verify(dbSet => dbSet.Add(newUser), Times.Once);
            mockContext.Verify(context => context.SaveChanges(), Times.Once);
        }
    }
}
