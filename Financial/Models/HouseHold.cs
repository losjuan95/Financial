using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Financial.Models
{
    public class HouseHold
    {
        public int Id { get; set; }

        public string Description { get; set; }

        [MaxLength(30), MinLength(4)]
        public string Name { get; set; }

        [MaxLength(30), MinLength(4)]
        public string Greeting { get; set; }

        public virtual ICollection<Budget> Budgets { get; set; }
        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<ApplicationUser> Members { get; set; }
        public virtual ICollection<Invitation> Invitations { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        
        public HouseHold()
        {
            Members = new HashSet<ApplicationUser>();
            Budgets = new HashSet<Budget>();
            Accounts = new HashSet<Account>();
            Notifications = new HashSet<Notification>();
            Invitations = new HashSet<Invitation>();
        }
    }
}