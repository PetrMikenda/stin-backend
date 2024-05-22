using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using stin_api.Models;
using stin_api.Controllers;

namespace Testing.ControllersTesting
{
    public class UserControllerTest
    {
        private readonly DbContextOptions<AppDbContext> _dbContextOptions;

        public UserControllerTest()
        {
            _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "UsersControllerTests")
                .Options;
        }

        [Fact]
        public async Task GetUser_ShouldReturnAllUsers()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new AppDbContext(dbContextOptions))
            {
                if (!context.Users.Any(u => u.id == 1))
                {
                    context.Users.Add(new User { id = 1, username = "Johnny", email = "john@email.com", password = "newpassword", premium = "true" });
                }

                if (!context.Users.Any(u => u.id == 2))
                {
                    context.Users.Add(new User { id = 2, username = "Jane", email = "email@email.cz", password = "password", premium = "true" });
                }

                context.SaveChanges();

                var controller = new UsersController(context);

                // Act
                var result = await controller.GetUsers();

                // Assert
                result.Value.Should().BeAssignableTo<IEnumerable<User>>();

                var users = result.Value as IEnumerable<User>;
                users.Should().HaveCount(2);
                users.Should().ContainEquivalentOf(
                    new User { id = 1, username = "Johnny", email = "john@email.com", password = "newpassword", premium = "true" },
                    options => options.ExcludingMissingMembers()
                );
                users.Should().ContainEquivalentOf(
                    new User { id = 2, username = "Jane", email = "email@email.cz", password = "password", premium = "true" },
                    options => options.ExcludingMissingMembers()
                );
            }
        }

        [Fact]
        public void UserModel_ShouldHaveCorrectProperties()
        {
            // Arrange
            var user = new User
            {
                id = 1,
                username = "TestUser",
                email = "testuser@example.com",
                password = "password123",
                premium = "ano"
            };

            // Act & Assert
            user.id.Should().Be(1);
            user.username.Should().Be("TestUser");
            user.email.Should().Be("testuser@example.com");
            user.password.Should().Be("password123");
            user.premium.Should().Be("ano");
        }

        [Fact]
        public void UserLoginModel_ShouldHaveCorrectProperties()
        {
            // Arrange
            var userLoginModel = new UserLoginModel
            {
                email = "testuser@example.com",
                password = "password123"
            };

            // Act & Assert
            userLoginModel.email.Should().Be("testuser@example.com");
            userLoginModel.password.Should().Be("password123");
        }

        [Fact]
        public void UserRegisterModel_ShouldHaveCorrectProperties()
        {
            // Arrange
            var userRegisterModel = new UserRegisterModel
            {
                username = "TestUser",
                email = "testuser@example.com",
                password = "password123"
            };

            // Act & Assert
            userRegisterModel.username.Should().Be("TestUser");
            userRegisterModel.email.Should().Be("testuser@example.com");
            userRegisterModel.password.Should().Be("password123");
        }
    }
}
