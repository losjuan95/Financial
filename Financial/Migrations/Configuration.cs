namespace Financial.Migrations
{
    using Financial.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Financial.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Financial.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>());

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }

            if (!context.Roles.Any(r => r.Name == "Member"))
            {
                roleManager.Create(new IdentityRole { Name = "Member" });
            }

            if (!context.Roles.Any(r => r.Name == "HOH"))
            {
                roleManager.Create(new IdentityRole { Name = "HOH" });
            }
            if (!context.Roles.Any(r => r.Name == "Guest"))
            {
                roleManager.Create(new IdentityRole { Name = "Guest" });
            }

            var userManager = new UserManager<ApplicationUser>(
              new UserStore<ApplicationUser>(context));
            if (!context.Users.Any(u => u.Email == "juancarloscorea95@gmail.com"))
            {

                userManager.Create(new ApplicationUser
                {
                    UserName = "juancarloscorea95@gmail.com",
                    Email = "juancarloscorea95@gmail.com",
                    FirstName = "Juan Carlos",
                    LastName = "Corea"


                }, "Hollirose95");
                var userId = userManager.FindByEmail("juancarloscorea95@gmail.com").Id;
                userManager.AddToRole(userId, "Admin");
            }
          
        }
    }
}
