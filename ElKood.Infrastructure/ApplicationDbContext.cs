using ElKood.Core.Entities;
using ElKood.Core.Entities.Identity;
using ElKood.Infrastructure.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ElKood.Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ToDoTask> ToDoTasks { get; set; }

        public DbSet<TaskCategory> TaskCategories { get; set; }

        public DbSet<AppLog> AppLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Write Fluent API configurations here
            new TaskCategoryConfiguration().Configure(modelBuilder.Entity<TaskCategory>());
            new ToDoTaskConfiguration().Configure(modelBuilder.Entity<ToDoTask>());
            var taskCategories = new List<TaskCategory>
            {
                new TaskCategory
                {
                    Id = 1,
                    Name = "Project Management",
                    Description = "Project Management"
                },
                new TaskCategory
                {
                    Id = 2,
                    Name = "Software Development",
                    Description = "Software Development"
                },
                new TaskCategory
                {
                    Id = 3,
                    Name = "Operations",
                    Description = "Operations"
                },
                new TaskCategory
                {
                    Id = 4,
                    Name = "Learning",
                    Description = "Learning"
                },
            };

            var tasks = new List<ToDoTask>
            {
                new ToDoTask
                {
                    Id = 1,
                    Name = "elkood assigement development.",
                    Description=  null,
                    Priority = 1 ,
                    CompletionDate = null,
                    CategoryId = 2
                },

                new ToDoTask
                {
                    Id = 2,
                    Name = "elkood assigement test.",
                    Description=  null,
                    Priority = 1 ,
                    CompletionDate = null,
                    CategoryId = 2
                },

                new ToDoTask
                {
                    Id = 3,
                    Name = "elkood assigement Docker operation",
                    Description=  null,
                    Priority = 2 ,
                    CompletionDate = null,
                    CategoryId = 3
                },

                new ToDoTask
                {
                    Id = 4,
                    Name = "ASP.Net Course.",
                    Description=  "Attend Online class",
                    Priority = 3 ,
                    CompletionDate = null,
                    CategoryId = 4
                },
            };
            modelBuilder.Entity<TaskCategory>(entity => entity.HasData(taskCategories));
            modelBuilder.Entity<ToDoTask>(entity => entity.HasData(tasks));

            // Seed Owner User & Roles OnModelCreating "UserName Not Found" issued// 
            //var hasher = new PasswordHasher<ApplicationUser>();
            //modelBuilder.Entity<ApplicationUser>(entity => entity.HasData(
            //    new ApplicationUser
            //    {
            //    Id = "9f312698-a4b5-485a-830e-b02021e89a1a",
            //    UserName = "Owner",
            //    Email = "Owner@elkoodtask.com",
            //    EmailConfirmed = true,
            //    PasswordHash = hasher.HashPassword(null,"Aa@123456")
            //    }));
            //modelBuilder.Entity<IdentityRole>(entity => entity.HasData(
            //    new IdentityRole
            //    {
            //        Id = "0ec22ebc-9bb1-429b-ade2-6ea9d6d27836",
            //        Name = "Owner",
            //        ConcurrencyStamp = null,
            //    },
            //    new IdentityRole
            //    {
            //        Id = "48744dfe-c312-4068-8ad2-b8a5f009ce43",
            //        Name = "Guest"
            //    }));
            //modelBuilder.Entity<IdentityUserRole<string>>
            //    (entity => entity.HasData(new IdentityUserRole<string>
            //    {
            //        UserId = "9f312698-a4b5-485a-830e-b02021e89a1a",
            //        RoleId = "0ec22ebc-9bb1-429b-ade2-6ea9d6d27836"
            //    }));

            base.OnModelCreating(modelBuilder);
        }
    }
}
