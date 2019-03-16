using Financial.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Financial.ViewModels
{
    public class Public
    {
        public virtual List<Budget> Budgets { get; set; }
        public virtual List<BudgetItem> BudgetItems { get; set; }
        public virtual List<Transaction> Transactions { get; set; }
        public virtual List<Account> Accounts { get; set; }
       
        
        public Public()
        {
            Budgets = new List<Budget>();
            BudgetItems = new List<BudgetItem>();
            Transactions = new List<Transaction>();
            Accounts = new List<Account>();
        }
    }

   
}