using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Financial.Models
{
    public class BudgetItem
    {
        public int Id { get; set; }

        [MaxLength(30), MinLength(4)]
        public string Name { get; set; }
        [MaxLength(200), MinLength(4)]
        public string Description { get; set; }

        public decimal TargetTotal { get; set; }
        public decimal CurrentTotal { get; set; }

        public int BudgetId { get; set; }

        public virtual Budget Budget { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }

        public BudgetItem()
        {
            Transactions = new HashSet<Transaction>();
        }

    
    }
}