using Financial.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Financial.ViewModels
{
    public class BudgetItemsViewModel
    {
        public List<BudgetItem> Food { get; set; } 
        public BudgetItemsViewModel()
        {
            Food = new List<BudgetItem>();
        }
    }
}