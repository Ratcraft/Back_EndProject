using Microsoft.EntityFrameworkCore;
using Models;

namespace BookController.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) {}
        public DbSet<Book> Book {get; set;}
        public DbSet<Book_description> Book_Description {get; set;}
        public DbSet<User> User {get; set;}
    }
}