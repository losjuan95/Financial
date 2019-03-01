using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Financial.Models
{
    public class Invitation
    {
        public int Id { get; set; }
        public int HouseHoldId { get; set; }

        [MaxLength(200), MinLength(4)]
        public string Body { get; set; }

        [Required]
        public Guid KeyCode { get; set; }

        [Required, EmailAddress]
        public string Email { get; set;}

        public DateTime Created { get; set; }
        public DateTime? Accepted { get; set; }
        public DateTime Expires { get; set; }

        public bool Expired { get; set; }

       public virtual HouseHold HouseHold { get; set; }
    }
}