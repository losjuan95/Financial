﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Financial.Helper;
using Financial.Models;
using Microsoft.AspNet.Identity;

namespace Financial.Controllers
{
    public class AccountsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private BankAccountHelper ba = new BankAccountHelper();
        private HouseHelper hou = new HouseHelper();
        // GET: Accounts
        public ActionResult Index()
        {
            var accounts = db.Accounts.Include(a => a.HouseHold);
            var useraccounts = ba.ListUserAccount(User.Identity.GetUserId()).ToList();
            return View(accounts.ToList());
        }

        // GET: Accounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            var budgetitems = new List<BudgetItem>();
            foreach(var budget in db.Budgets.Where(b => b.HouseHoldId == account.HouseHoldId).ToList())
            {
                budgetitems.AddRange(budget.BudgetItems);
            }

            ViewBag.BudgetItemId = new SelectList( budgetitems, "Id", "Name");

            return View(account);
        }

        // GET: Accounts/Create
        [Authorize(Roles = "HOH, Member")]
        public ActionResult Create()
        {
            var houseId = db.Users.Find(User.Identity.GetUserId()).HouseHoldId;
            var newaccount = new Account
            {
                HouseHoldId = (int)houseId
            };
            return View(newaccount);
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,HouseHoldId,InitialBalance,CurrentBalance,ReconciledBalance,LowBalanceLimit")] Account account)
        {
            if (ModelState.IsValid)
            {
                db.Accounts.Add(account);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var house = hou.ListUserHouse();

            ViewBag.HouseHoldId = new SelectList( house, "Id", "Description", account.HouseHoldId);
            return View(account);
        }

        // GET: Accounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,HouseHoldId,InitialBalance,CurrentBalance,ReconciledBalance,LowBalanceLimit")] Account account)
        {
            if (ModelState.IsValid)
            {
                db.Entry(account).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HouseHoldId = new SelectList(db.HouseHolds, "Id", "Description", account.HouseHoldId);
            return View(account);
        }

        // GET: Accounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Account account = db.Accounts.Find(id);
            db.Accounts.Remove(account);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
