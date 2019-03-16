using Financial.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Financial.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int AccountId { get; set; }

        [MaxLength(30), MinLength(4)]
        public string Description { get; set; }
        public DateTime Date { get; set; }

        public decimal Amount { get; set; }
        public TransactionType Type { get; set; }

        public string EnteredById { get; set; }

        public bool Reconciled { get; set; }
        public decimal ReconciledAmount { get; set; }
       
        public virtual Account Account { get; set; }
        public virtual ApplicationUser Enteredby { get; set; }

        public int? BudgetItemId { get; set; }
       
        public virtual BudgetItem BudgetItem { get; set; }
        
    }
}