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

        //public void AddUserToHouse(ApplicationDbContext context, string houseHold, string email)
        //{
        //    var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
        //    var user = userManager.FindByEmail(email);

        //    var HouseholdId = context.HouseHolds.SingleOrDefault(c => c.Name == houseHold);
        //    HouseholdId.Members.Add(user);
        //}

        protected override void Seed(Financial.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
          
            //context.BudgetItems.AddOrUpdate(m => m.Name,
            //  new BudgetItem() { Name = "Electrical bill" },
            //  new BudgetItem() { Name = "Water Bill" },
            //  new BudgetItem() { Name = "Groceries" },
            //  new BudgetItem() { Name = "Dining out" },
            //  new BudgetItem() { Name = "Emergency" },
            //  new BudgetItem() { Name = "Car Bill" },
            //  new BudgetItem() { Name = "Gas" },
            //  new BudgetItem() { Name = "Emergencies" }



            //  );
            //context.Budgets.AddOrUpdate(m => m.Name,
            //    new Budget() { Name = "Utilities" },
            //    new Budget() { Name = "Mortgage" },
            //    new Budget() { Name = "Food" },
            //    new Budget() { Name = "401k" }

            //    );
            //context.Accounts.AddOrUpdate(m => m.Name,
            //    new Account() { Name = "Checking" },
            //    new Account() { Name = "Savings" },
            //    new Account() { Name = "Retirement" }
            //    );
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

            var userManager = new UserManager<ApplicationUser>( new UserStore<ApplicationUser>(context));
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
            if (!context.Users.Any(u => u.Email == "DemoHOH@Mailinator.com"))
            {

                userManager.Create(new ApplicationUser
                {
                    UserName = "DemoHOH@Mailinator.com",
                    Email = "DemoHOH@Mailinator.com",
                    FirstName = "HOH",
                    LastName = "Demo"


                }, "Abc&123!");
                var userId = userManager.FindByEmail("DemoHOH@Mailinator.com").Id;
                userManager.AddToRole(userId, "HOH");


            }
            if (!context.Users.Any(u => u.Email == "DemoGuest@Mailinator.com"))
            {

                userManager.Create(new ApplicationUser
                {
                    UserName = "DemoGuest@Mailinator.com",
                    Email = "DemoGuest@Mailinator.com",
                    FirstName = "Guest",
                    LastName = "Demo"


                }, "Abc&123!");
                var userId = userManager.FindByEmail("DemoGuest@Mailinator.com").Id;
                userManager.AddToRole(userId, "Guest");


            }
           


        }
    }
}
