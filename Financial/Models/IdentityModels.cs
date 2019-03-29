using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Financial.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "First Name")]
        [Required, MaxLength(20), MinLength(1)]
        public string FirstName { get; set; }

        [Display(Name ="Last Name")]
        [Required, MaxLength(20), MinLength(1)]
        public string LastName { get; set; }
        
        public string AvatarPath { get; set; }

        public int? HouseHoldId { get; set; }

        public virtual HouseHold HouseHold { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }
       
        public virtual ICollection<Notification> Notifications { get; set; }

        public ApplicationUser()
        {
            Transactions = new HashSet<Transaction>();
            Notifications = new HashSet<Notification>();
        }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<HouseHold> HouseHolds { get; set; }

        public DbSet<Budget> Budgets { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<BudgetItem> BudgetItems { get; set; }
        
        public DbSet<Notification> Notifications { get; set; }

        public DbSet<Invitation> Invitations { get; set; }
        public object Households { get; internal set; }
    }
}