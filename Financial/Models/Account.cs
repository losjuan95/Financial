using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Financial.Models
{
    public class Account
    {
        public int Id { get; set; }

        [MaxLength(30), MinLength(4)]
        public string Name { get; set; }

        public int HouseHoldId { get; set; }

        public decimal InitialBalance { get; set; }

        public decimal CurrentBalance { get; set; }
        public decimal ReconciledBalance { get; set; }

        public decimal LowBalanceLimit { get; set; }
        

        public virtual HouseHold HouseHold { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }

        public Account()
        {
            Transactions = new HashSet<Transaction>();
        }
    }
}