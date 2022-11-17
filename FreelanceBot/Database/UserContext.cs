
using FreelanceBot.Models;
using Microsoft.EntityFrameworkCore;

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
            optionsBuilder.UseMySql(@"server=mysql.j91146254.myjino.ru;user=j91146254;password=kompass84;database=j91146254_jobs;");
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Job> Jobs { get; set; }

        public DbSet<Resume> Resumes { get; set; }
        public DbSet<Event> Events { get; set; }


    }
}
