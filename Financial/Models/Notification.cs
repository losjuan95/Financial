using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Financial.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public int? HouseHoldId { get; set; }

        [Required]
        public string RecipientId { get; set; }
        public bool Warning { get; set; }

        public string Body { get; set; }
        public bool Read { get; set; }
        public bool Confirmed { get; set; }

        
        
        public virtual HouseHold HouseHold { get; set; }
        public virtual ApplicationUser Recipient { get; set; }
        
    }
}