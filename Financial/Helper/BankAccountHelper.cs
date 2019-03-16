using Financial.Enumerations;
using Financial.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Financial.Helper
{
    public class BankAccountHelper
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        public static decimal GetCurrentBalance(int bankId)
        {
            var transactions = db.Transactions.Where(t => t.AccountId == bankId);
            var deposit = transactions.Where(t => t.Type == TransactionType.Deposit).Sum(t => t.Amount);
            var withdrawl = transactions.Where(t => t.Type == TransactionType.Withdrawal && t.Amount > 0M).ToList();

            decimal totalwithdrawl = 0M;

            if (withdrawl != null)
            {
                totalwithdrawl = withdrawl.Where(w => w.Amount != 0M).Sum(w => w.Amount);
            }

            var account = db.Accounts.Find(bankId);

            if (withdrawl.Count() > 0)
            {
                totalwithdrawl = withdrawl.Sum(w => w.Amount);

            }


            return account.InitialBalance + deposit - totalwithdrawl;
        }

        public ICollection<Account> ListUserAccount(string userId)
        {
            ApplicationUser user = db.Users.Find(userId);

            var accounts = user.HouseHold.Accounts.ToList();
            return (accounts);
        }
    }
}