
using FreelanceBot.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace FreelanceBot.Database
{
    public class UserContext : DbContext
    {
        public UserContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(@"server=vh70.timeweb.ru;user=cj49109_jobevent;password=SLabWS3f;database=cj49109_jobevent;", new MySqlServerVersion(new Version(5, 6, 0)), options => options.EnableRetryOnFailure());
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Job> Jobs { get; set; }

        public DbSet<Resume> Resumes{ get; set; }
        public DbSet<Event> Events{ get; set; }


    }
}
