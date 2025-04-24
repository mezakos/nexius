using Microsoft.EntityFrameworkCore;
using Nexius.API.DAL.Models;

namespace Nexius.API.DAL
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options) : base(options)
        {
        }

        public DbSet<TodoItem> TodoItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultContainer(nameof(TodoItems));

            modelBuilder.Entity<TodoItem>()
                .HasNoDiscriminator()
                .ToContainer(nameof(TodoItems))
                .HasKey(x => x.Id);

        }
    }
}
