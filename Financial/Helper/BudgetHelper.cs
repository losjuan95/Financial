using Financial.Enumerations;
using Financial.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Financial.Helper
{
    public class BudgetHelper
    {
       private static ApplicationDbContext db = new ApplicationDbContext();

        public static int GetBudgetIdFromName(string budgetName)
        {
            var budgetId = -1;
            var budget = db.Budgets.FirstOrDefault(b => b.Name == budgetName);

            if (budget != null)
                budgetId = budget.Id;
            return budgetId;
        }


        public static decimal GetBudgetTarget(int budgetId)
        {
            return db.BudgetItems.Where(b => b.BudgetId == budgetId).Sum(b => b.TargetTotal);

        }

        public static decimal GetBudgetActual(int budgetId)
        {
            decimal? actual = 0M;

            var budgetItems = db.BudgetItems.Where(b => b.BudgetId == budgetId);


            foreach (var budgetItem in budgetItems.ToList())
            {
               var transactions = db.Transactions.Where(t => t.BudgetItemId == budgetItem.Id && t.Type == TransactionType.Withdrawal && t.Amount > 0M).ToList();
                if (transactions.Count > 0)
                    actual += transactions.Sum(t => t.Amount);

            }

            return (decimal)actual;

        }
    }
}