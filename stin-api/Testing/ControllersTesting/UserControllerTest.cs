using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using stin_api.Models;
using stin_api.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Testing.ControllersTesting
{
    public class UserControllerTest
    {
        private readonly DbContextOptions<AppDbContext> _dbContextOptions;

        private DbContextOptions<AppDbContext> GetDbContextOptions()
        {
            return new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // Use a new database for each test
                .Options;
        }

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

        
        [Fact]
        public async Task GetUser_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var dbContextOptions = GetDbContextOptions();

            using (var context = new AppDbContext(dbContextOptions))
            {
                var testUser = new User { id = 1, username = "TestUser", email = "testuser@example.com", password = "password123", premium = "ne" };
                context.Users.Add(testUser);
                context.SaveChanges();

                var controller = new UsersController(context);

                // Act
                var result = await controller.GetUser(1);

                // Assert
                result.Result.Should().BeNull(); // Since result.Result is null, directly check the value
                var user = result.Value;
                user.Should().BeEquivalentTo(testUser, options => options.ExcludingMissingMembers());
            }
        }

        [Fact]
        public async Task GetUser_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new AppDbContext(dbContextOptions))
            {
                var controller = new UsersController(context);

                // Act
                var result = await controller.GetUser(999); // Non-existent user ID

                // Assert
                result.Result.Should().BeOfType<NotFoundResult>();
            }
        }

        [Fact]
        public async Task PutUser_ShouldReturnNoContent_WhenUserIsUpdated()
        {
            // Arrange
            var dbContextOptions = GetDbContextOptions();

            using (var context = new AppDbContext(dbContextOptions))
            {
                var testUser = new User { id = 1, username = "TestUser", email = "testuser@example.com", password = "password123", premium = "ne" };
                context.Users.Add(testUser);
                context.SaveChanges();

                var controller = new UsersController(context);

                var updatedUser = new User { id = 1, username = "UpdatedUser", email = "updateduser@example.com", password = "newpassword", premium = "ano" };

                // Act
                // Detach the existing entity
                context.Entry(testUser).State = EntityState.Detached;
                var result = await controller.PutUser(1, updatedUser);

                // Assert
                result.Should().BeOfType<NoContentResult>();
                var user = await context.Users.FindAsync(1);
                user.Should().BeEquivalentTo(updatedUser, options => options.ExcludingMissingMembers());
            }
        }

        [Fact]
        public async Task PutUser_ShouldReturnBadRequest_WhenIdDoesNotMatch()
        {
            // Arrange
            var dbContextOptions = GetDbContextOptions();

            using (var context = new AppDbContext(dbContextOptions))
            {
                var testUser = new User { id = 1, username = "TestUser", email = "testuser@example.com", password = "password123", premium = "ne" };
                context.Users.Add(testUser);
                context.SaveChanges();

                var controller = new UsersController(context);

                var updatedUser = new User { id = 2, username = "UpdatedUser", email = "updateduser@example.com", password = "newpassword", premium = "ano" };

                // Act
                var result = await controller.PutUser(1, updatedUser);

                // Assert
                result.Should().BeOfType<BadRequestResult>();
            }
        }

        [Fact]
        public async Task PutUser_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var dbContextOptions = GetDbContextOptions();

            using (var context = new AppDbContext(dbContextOptions))
            {
                var controller = new UsersController(context);

                var updatedUser = new User { id = 999, username = "UpdatedUser", email = "updateduser@example.com", password = "newpassword", premium = "ano" };

                // Act
                var result = await controller.PutUser(999, updatedUser);

                // Assert
                result.Should().BeOfType<NotFoundResult>();
            }
        }

        [Fact]
        public async Task PostUser_ShouldCreateUser_AndReturnCreatedAtAction()
        {
            // Arrange
            var dbContextOptions = GetDbContextOptions();

            using (var context = new AppDbContext(dbContextOptions))
            {
                var controller = new UsersController(context);
                var newUser = new User { id = 1, username = "NewUser", email = "newuser@example.com", password = "password123", premium = "ne" };

                // Act
                var result = await controller.PostUser(newUser);

                // Assert
                result.Result.Should().BeOfType<CreatedAtActionResult>();
                var createdAtActionResult = result.Result as CreatedAtActionResult;
                createdAtActionResult.ActionName.Should().Be("GetUser");
                createdAtActionResult.RouteValues["id"].Should().Be(newUser.id);
                var createdUser = createdAtActionResult.Value as User;
                createdUser.Should().BeEquivalentTo(newUser, options => options.ExcludingMissingMembers());

                // Verify the user was added to the database
                var userInDb = await context.Users.FindAsync(newUser.id);
                userInDb.Should().NotBeNull();
                userInDb.Should().BeEquivalentTo(newUser, options => options.ExcludingMissingMembers());
            }
        }

        [Fact]
        public async Task DeleteUser_ShouldReturnNoContent_WhenUserExists()
        {
            // Arrange
            var dbContextOptions = GetDbContextOptions();

            using (var context = new AppDbContext(dbContextOptions))
            {
                var testUser = new User { id = 1, username = "TestUser", email = "testuser@example.com", password = "password123", premium = "ne" };
                context.Users.Add(testUser);
                context.SaveChanges();

                var controller = new UsersController(context);

                // Act
                var result = await controller.DeleteUser(1);

                // Assert
                result.Should().BeOfType<NoContentResult>();

                // Verify the user was removed from the database
                var userInDb = await context.Users.FindAsync(1);
                userInDb.Should().BeNull();
            }
        }

        [Fact]
        public async Task DeleteUser_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var dbContextOptions = GetDbContextOptions();

            using (var context = new AppDbContext(dbContextOptions))
            {
                var controller = new UsersController(context);

                // Act
                var result = await controller.DeleteUser(999); // Non-existent user ID

                // Assert
                result.Should().BeOfType<NotFoundResult>();
            }
        }

        [Fact]
        public async Task UserPay_ShouldReturnNoContent_WhenUserExists()
        {
            // Arrange
            var dbContextOptions = GetDbContextOptions();

            using (var context = new AppDbContext(dbContextOptions))
            {
                var testUser = new User { id = 1, username = "TestUser", email = "testuser@example.com", password = "password123", premium = "ne" };
                context.Users.Add(testUser);
                context.SaveChanges();

                var controller = new UsersController(context);

                // Act
                var result = await controller.UserPay(1);

                // Assert
                result.Should().BeOfType<NoContentResult>();

                // Verify the user's premium status was updated in the database
                var userInDb = await context.Users.FindAsync(1);
                userInDb.premium.Should().Be("ano");
            }
        }

        [Fact]
        public async Task UserPay_ShouldReturnBadRequest_WhenUserDoesNotExist()
        {
            // Arrange
            var dbContextOptions = GetDbContextOptions();

            using (var context = new AppDbContext(dbContextOptions))
            {
                var controller = new UsersController(context);

                // Act
                var result = await controller.UserPay(999); // Non-existent user ID

                // Assert
                result.Should().BeOfType<BadRequestResult>();
            }
        }

        [Fact]
        public async Task UserRegister_ShouldCreateUser_WhenEmailDoesNotExist()
        {
            // Arrange
            var dbContextOptions = GetDbContextOptions();

            using (var context = new AppDbContext(dbContextOptions))
            {
                var controller = new UsersController(context);
                var newUser = new UserRegisterModel
                {
                    username = "NewUser",
                    email = "newuser@example.com",
                    password = "password123"
                };

                // Act
                var result = await controller.UserRegister(newUser);

                // Assert
                result.Result.Should().BeOfType<CreatedAtActionResult>();
                var createdAtActionResult = result.Result as CreatedAtActionResult;
                createdAtActionResult.ActionName.Should().Be("GetUser");
                var user = createdAtActionResult.Value as User;
                user.Should().NotBeNull();
                user.username.Should().Be(newUser.username);
                user.email.Should().Be(newUser.email);
                user.password.Should().Be(newUser.password);
                user.premium.Should().Be("ne");

                // Verify the user was added to the database
                var userInDb = await context.Users.FindAsync(user.id);
                userInDb.Should().NotBeNull();
                userInDb.Should().BeEquivalentTo(user, options => options.ExcludingMissingMembers());
            }
        }

        [Fact]
        public async Task UserRegister_ShouldReturnBadRequest_WhenEmailAlreadyExists()
        {
            // Arrange
            var dbContextOptions = GetDbContextOptions();

            using (var context = new AppDbContext(dbContextOptions))
            {
                var existingUser = new User { id = 1, username = "ExistingUser", email = "existinguser@example.com", password = "password123", premium = "ne" };
                context.Users.Add(existingUser);
                context.SaveChanges();

                var controller = new UsersController(context);
                var newUser = new UserRegisterModel
                {
                    username = "NewUser",
                    email = "existinguser@example.com", // Same email as existing user
                    password = "password123"
                };

                // Act
                var result = await controller.UserRegister(newUser);

                // Assert
                result.Result.Should().BeOfType<BadRequestObjectResult>();
                var badRequestResult = result.Result as BadRequestObjectResult;
                badRequestResult.Value.Should().Be("User with this email exists");

                // Verify the user was not added to the database
                var usersInDb = context.Users.Where(u => u.email == newUser.email).ToList();
                usersInDb.Should().HaveCount(1);
                usersInDb[0].Should().BeEquivalentTo(existingUser, options => options.ExcludingMissingMembers());
            }
        }

        [Fact]
        public async Task UserLogin_ShouldReturnUser_WhenCredentialsAreCorrect()
        {
            // Arrange
            var dbContextOptions = GetDbContextOptions();

            using (var context = new AppDbContext(dbContextOptions))
            {
                var testUser = new User { id = 1, username = "TestUser", email = "testuser@example.com", password = "password123", premium = "ne" };
                context.Users.Add(testUser);
                context.SaveChanges();

                var controller = new UsersController(context);
                var loginModel = new UserLoginModel
                {
                    email = "testuser@example.com",
                    password = "password123"
                };

                // Act
                var result = await controller.UserLogin(loginModel);

                // Assert
                result.Result.Should().BeNull(); // Since result.Result is null, directly check the value
                var user = result.Value;
                user.Should().BeEquivalentTo(testUser, options => options.ExcludingMissingMembers());
            }
        }

        [Fact]
        public async Task UserLogin_ShouldReturnNotFound_WhenCredentialsAreIncorrect()
        {
            // Arrange
            var dbContextOptions = GetDbContextOptions();

            using (var context = new AppDbContext(dbContextOptions))
            {
                var testUser = new User { id = 1, username = "TestUser", email = "testuser@example.com", password = "password123", premium = "ne" };
                context.Users.Add(testUser);
                context.SaveChanges();

                var controller = new UsersController(context);
                var loginModel = new UserLoginModel
                {
                    email = "testuser@example.com",
                    password = "wrongpassword"
                };

                // Act
                var result = await controller.UserLogin(loginModel);

                // Assert
                result.Result.Should().BeOfType<NotFoundResult>();
            }
        }
    }
}
