using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using stin_api.Models;

namespace stin_api.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<stin_api.Models.Favorite> Favorites { get; set; } = default!;
    }
}
