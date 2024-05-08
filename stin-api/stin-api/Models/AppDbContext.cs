using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace stin_api.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
    }
}
