
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
            optionsBuilder.UseMySql(@"server=mysql.j81076941.myjino.ru;user=j81076941;password=448agiAoi;database=j81076941_jobs;");
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Job> Jobs { get; set; }

        public DbSet<Resume> Resumes{ get; set; }
        public DbSet<Event> Events{ get; set; }


    }
}
