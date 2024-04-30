using Microsoft.EntityFrameworkCore;
using BookManagementAPI.Models;

namespace BookManagementAPI
{
    public class ApplicationDbContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Admin> Admins { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public ApplicationDbContext()
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder) //Augustas: for user roles functionality
        {
            base.OnModelCreating(modelBuilder);
           
            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<int>();            
        }
    }
}
