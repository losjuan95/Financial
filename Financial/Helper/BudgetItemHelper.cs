using Financial.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Financial.Helper
{
    public class BudgetItemHelper
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        public static int GetBudgetItemIdFromName(string budgetItemName)
        {
            var budgetItemId = -1;
            var budgetItem = db.BudgetItems.FirstOrDefault(b => b.Name == budgetItemName);


            if (budgetItem != null)
                budgetItemId = budgetItem.Id;
            return budgetItemId;
        }
    }
}