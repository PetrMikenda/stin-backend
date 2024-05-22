using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using stin_api.Controllers;
using stin_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testing.ControllersTesting
{
    public class FavoriteControllerTest
    {
        private DbContextOptions<AppDbContext> GetDbContextOptions()
        {
            return new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // Use a new database for each test
                .Options;
        }

        [Fact]
        public async Task GetFavorites_ShouldReturnFavorites_WhenFavoritesExist()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new AppDbContext(dbContextOptions))
            {
                var testFavorites = new List<Favorite>
        {
            new Favorite { id = 1, city = "New York", Users_id = 1 },
            new Favorite { id = 2, city = "Los Angeles", Users_id = 2 }
        };
                context.Favorites.AddRange(testFavorites);
                context.SaveChanges();

                var controller = new FavoritesController(context);

                // Act
                var result = await controller.GetFavorites();

                // Assert
                result.Should().NotBeNull();
                var favorites = result.Value;
                favorites.Should().BeEquivalentTo(testFavorites, options => options.ExcludingMissingMembers());
            }
        }

        [Fact]
        public async Task GetFavorites_ShouldReturnEmptyList_WhenNoFavoritesExist()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new AppDbContext(dbContextOptions))
            {
                // Ensure the database is empty
                context.Favorites.RemoveRange(context.Favorites);
                context.SaveChanges();

                var controller = new FavoritesController(context);

                // Act
                var result = await controller.GetFavorites();

                // Assert
                result.Should().NotBeNull();
                var favorites = result.Value;
                favorites.Should().BeEmpty();
            }
        }

        [Fact]
        public async Task GetFavorite_ShouldReturnFavorite_WhenFavoriteExists()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new AppDbContext(dbContextOptions))
            {
                var testFavorite = new Favorite { id = 1, city = "New York", Users_id = 1 };
                context.Favorites.Add(testFavorite);
                context.SaveChanges();

                var controller = new FavoritesController(context);

                // Act
                var result = await controller.GetFavorite(1);

                // Assert
                result.Result.Should().BeNull(); // Since result.Result is null, directly check the value
                var favorite = result.Value;
                favorite.Should().BeEquivalentTo(testFavorite, options => options.ExcludingMissingMembers());
            }
        }

        [Fact]
        public async Task GetFavorite_ShouldReturnNotFound_WhenFavoriteDoesNotExist()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new AppDbContext(dbContextOptions))
            {
                var controller = new FavoritesController(context);

                // Act
                var result = await controller.GetFavorite(1);

                // Assert
                result.Result.Should().BeOfType<NotFoundResult>();
            }
        }

        [Fact]
        public async Task PutFavorite_ShouldReturnNoContent_WhenFavoriteIsUpdated()
        {
            // Arrange
            var dbContextOptions = GetDbContextOptions();
            using (var context = new AppDbContext(dbContextOptions))
            {
                var testFavorite = new Favorite { id = 1, city = "New York", Users_id = 1 };
                context.Favorites.Add(testFavorite);
                context.SaveChanges();

                var controller = new FavoritesController(context);
                var updatedFavorite = new Favorite { id = 1, city = "Los Angeles", Users_id = 1 };

                // Act
                // Detach the existing entity to avoid tracking conflicts
                context.Entry(testFavorite).State = EntityState.Detached;

                var result = await controller.PutFavorite(1, updatedFavorite);

                // Assert
                result.Should().BeOfType<NoContentResult>();

                var favoriteInDb = await context.Favorites.FindAsync(1);
                favoriteInDb.Should().BeEquivalentTo(updatedFavorite, options => options.ExcludingMissingMembers());
            }
        }

        [Fact]
        public async Task PutFavorite_ShouldReturnBadRequest_WhenIdDoesNotMatch()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new AppDbContext(dbContextOptions))
            {
                var testFavorite = new Favorite { id = 1, city = "New York", Users_id = 1 };
                context.Favorites.Add(testFavorite);
                context.SaveChanges();

                var controller = new FavoritesController(context);
                var updatedFavorite = new Favorite { id = 2, city = "Los Angeles", Users_id = 1 };

                // Act
                var result = await controller.PutFavorite(1, updatedFavorite);

                // Assert
                result.Should().BeOfType<BadRequestResult>();
            }
        }

        [Fact]
        public async Task PutFavorite_ShouldReturnNotFound_WhenFavoriteDoesNotExist()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new AppDbContext(dbContextOptions))
            {
                var controller = new FavoritesController(context);
                var updatedFavorite = new Favorite { id = 1, city = "Los Angeles", Users_id = 1 };

                // Act
                var result = await controller.PutFavorite(1, updatedFavorite);

                // Assert
                result.Should().BeOfType<NotFoundResult>();
            }
        }

        [Fact]
        public async Task PostFavorite_ShouldCreateFavorite_WhenFavoriteIsValid()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new AppDbContext(dbContextOptions))
            {
                var controller = new FavoritesController(context);
                var newFavorite = new Favorite { id = 1, city = "New York", Users_id = 1 };

                // Act
                var result = await controller.PostFavorite(newFavorite);

                // Assert
                var actionResult = result.Result as CreatedAtActionResult;
                actionResult.Should().NotBeNull();
                actionResult.StatusCode.Should().Be(201);
                actionResult.Value.Should().BeEquivalentTo(newFavorite, options => options.ExcludingMissingMembers());

                var favoriteInDb = await context.Favorites.FindAsync(newFavorite.id);
                favoriteInDb.Should().BeEquivalentTo(newFavorite, options => options.ExcludingMissingMembers());
            }
        }
        [Fact]
        public async Task PostFavorite_ShouldReturnBadRequest_WhenFavoriteIsNull()
        {
            // Arrange
            var dbContextOptions = GetDbContextOptions();

            using (var context = new AppDbContext(dbContextOptions))
            {
                var controller = new FavoritesController(context);

                // Simulate model validation failure
                controller.ModelState.AddModelError("Favorite", "Favorite is required");

                // Act
                var result = await controller.PostFavorite(null);

                // Assert
                result.Result.Should().BeOfType<BadRequestResult>();
            }
        }

        [Fact]
        public async Task DeleteFavorite_ShouldReturnNoContent_WhenFavoriteIsDeleted()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new AppDbContext(dbContextOptions))
            {
                var testFavorite = new Favorite { id = 1, city = "New York", Users_id = 1 };
                context.Favorites.Add(testFavorite);
                context.SaveChanges();

                var controller = new FavoritesController(context);

                // Act
                var result = await controller.DeleteFavorite(1);

                // Assert
                result.Should().BeOfType<NoContentResult>();

                var favoriteInDb = await context.Favorites.FindAsync(1);
                favoriteInDb.Should().BeNull();
            }
        }

        [Fact]
        public async Task DeleteFavorite_ShouldReturnNotFound_WhenFavoriteDoesNotExist()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new AppDbContext(dbContextOptions))
            {
                var controller = new FavoritesController(context);

                // Act
                var result = await controller.DeleteFavorite(1);

                // Assert
                result.Should().BeOfType<NotFoundResult>();
            }
        }

        [Fact]
        public async Task GetUsersFavorites_ShouldReturnFavorites_WhenFavoritesExistForUser()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new AppDbContext(dbContextOptions))
            {
                var testFavorites = new List<Favorite>
            {
                new Favorite { id = 1, city = "New York", Users_id = 1 },
                new Favorite { id = 2, city = "Los Angeles", Users_id = 1 },
                new Favorite { id = 3, city = "Chicago", Users_id = 2 }
            };
                context.Favorites.AddRange(testFavorites);
                context.SaveChanges();

                var controller = new FavoritesController(context);

                // Act
                var result = await controller.GetUsersFavorites(1);

                // Assert
                result.Should().NotBeNull();
                var favorites = result.Value;
                favorites.Should().BeEquivalentTo(testFavorites.Where(f => f.Users_id == 1).ToList(), options => options.ExcludingMissingMembers());
            }
        }

        [Fact]
        public async Task GetUsersFavorites_ShouldReturnEmptyList_WhenNoFavoritesExistForUser()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new AppDbContext(dbContextOptions))
            {
                var controller = new FavoritesController(context);

                // Act
                var result = await controller.GetUsersFavorites(1);

                // Assert
                result.Should().NotBeNull();
                var favorites = result.Value;
                favorites.Should().BeEmpty();
            }
        }

    }
}
