using Microsoft.EntityFrameworkCore;
using Models;

namespace Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) {}

        public DbSet<User> User {get;set;}

        public DbSet<Employer> Employer {get;set;}

        public DbSet<Joboffer> Joboffer {get;set;}

        public DbSet<ApplyingJob> ApplyingJob { get; set; }
        public DbSet<Message> Message {get;set;}
        public DbSet<Rating> Rating { get; set; }
    }
}