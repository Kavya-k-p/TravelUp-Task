
using TravelUp_Task.Models; // Adjust based on your actual project structure
using Microsoft.EntityFrameworkCore;


namespace TravelUp_Task.Data;

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Item> Items { get; set; }
    }
