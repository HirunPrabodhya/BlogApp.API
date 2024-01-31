using Microsoft.EntityFrameworkCore;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class PostDBContext : DbContext
    {
        public DbSet<Post> posts { get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<UserType> usersType { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Subcribers> subcribers { get; set; }

        public PostDBContext(DbContextOptions<PostDBContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserType>().HasData(new UserType
            {
                Id = 1,
                Name = "Admin"
            },
            new UserType
            {
                Id = 2,
                Name = "Author"
            });
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 1,
                FirstName = "Hirun",
                LastName = "Prabodhya",
                Email = "hirunprabodhya@gmail.com",
                Password = "Hirun@123",
                UserTypeId = 1
            });
        }
    }
}
