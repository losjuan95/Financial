using Financial.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Financial.Helper
{

    public class HouseHelper
    {
        static  ApplicationDbContext db = new ApplicationDbContext();

        public bool IsUserOnHouse(string userId, int householdid)
        {
            var user = db.Users.Find(userId);

            if(user == null)
            {
                return false;
                
            }
            else
            {
                var houseId = user.HouseHoldId;
                return (houseId == householdid);
            }
            
           
           
        }
        public void RemoveUserFromHouse(string userId, int householdid)
        {
            if (IsUserOnHouse(userId, householdid))
            {
                HouseHold house = db.HouseHolds.Find(householdid);
                var delUser = db.Users.Find(userId);

                house.Members.Remove(delUser);
                //db.Entry(house).State = EntityState.Modified; 
                db.SaveChanges();

            }
        }

        public ICollection<ApplicationUser> UsersOnHouse(int HouseholdId)
        {
            return db.HouseHolds.Find(HouseholdId).Members.ToList();
        }


        public ICollection<HouseHold> ListUserHouse()
        {
            var user = HttpContext.Current.User.Identity.GetUserId();
            var house = db.Users.Find(user).HouseHoldId;

            var household = db.HouseHolds.Where(t => t.Id == house).ToList();

            return (household);
        }

        public int? GetUserHouseId()
        {
            var userid = HttpContext.Current.User.Identity.GetUserId();
            var houseId = db.Users.Find(userid).HouseHoldId;
            
            
            return (houseId);
        }

        public ICollection<Budget> ListUserBudgets()
        {
            var user = HttpContext.Current.User.Identity.GetUserId();
            var house = db.Users.Find(user).HouseHoldId;

            var Bud = db.Budgets.Where(b => b.HouseHoldId == house).ToList();
            return (Bud);
        }

        public ICollection<Transaction> ListUserTransactions()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
           
            var trans = db.Transactions.Where(t => t.EnteredById == userId).ToList();
            return (trans);
        }

        public ICollection<Transaction> ListHouseHoldTrans()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var householdid = db.Users.Find(userId).HouseHoldId;

            var trans = db.HouseHolds.Find(householdid).Accounts.SelectMany(a => a.Transactions).ToList();

            return (trans);
        }
        public ICollection<Account> ListAccounts()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var householdid = db.Users.Find(userId).HouseHoldId;

            var account = db.HouseHolds.Find(householdid).Accounts.ToList();

            return (account);
        }
        public ICollection<BudgetItem> ListBudgetItems()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var householdid = db.Users.Find(userId).HouseHoldId;

            var bud = db.HouseHolds.Find(householdid).Budgets.SelectMany(b => b.BudgetItems).ToList();

           
            return (bud);
        }
        //TODO
        public List<ApplicationUser> ListMembers()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var householdid = db.Users.Find(userId).HouseHoldId;

            var mem = db.HouseHolds.Find(householdid).Members.ToList();

            return (mem);
        }

        public static List<ApplicationUser> MemberList(int id)
        {
            return db.HouseHolds.Find(id).Members.ToList();
        }

    }
}