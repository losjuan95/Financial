﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Financial.Models;
using Microsoft.AspNet.Identity;

namespace Financial.Controllers
{

    public class TransactionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Transactions
        [Authorize(Roles = "HOH, Member")]
        public ActionResult Index()
        {
            var transactions = db.Transactions.Include(t => t.Account).Include(t => t.BudgetItem).Include(t => t.Enteredby);
            return View(transactions.ToList());
        }

        // GET: Transactions/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // GET: Transactions/Create
        [Authorize(Roles ="HOH, Member")]
        public ActionResult Create()
        {
         
            ViewBag.AccountId = new SelectList(db.Accounts, "Id", "Name");
            ViewBag.BudgetItemId = new SelectList(db.BudgetItems, "Id", "Name");
            return View();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Type,AccountId,Description,Amount,Reconciled,ReconciledAmount,BudgetItemId,Date")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                transaction.EnteredById = User.Identity.GetUserId();
                transaction.Date = DateTime.Now;
                
                db.Transactions.Add(transaction);
                db.SaveChanges();

                var account = db.Accounts.Find(transaction.AccountId);
                if(transaction.Type == Enumerations.TransactionType.Deposit)
                {
                    account.CurrentBalance += transaction.Amount;
                }
                else
                {
                    account.CurrentBalance -= transaction.Amount;
                }
                db.SaveChanges();
            }
            else
            {
                var message = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, message);
            }

            ViewBag.AccountId = new SelectList(db.Accounts, "Id", "Name", transaction.AccountId);
            ViewBag.BudgetItemId = new SelectList(db.BudgetItems, "Id", "Name", transaction.BudgetItemId);
            return RedirectToAction("Details", "Accounts", new { id = transaction.AccountId} );

        }

        // GET: Transactions/Edit/5
        [Authorize(Roles = "HOH, Member")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccountId = new SelectList(db.Accounts, "Id", "Name", transaction.AccountId);
            ViewBag.BudgetItemId = new SelectList(db.BudgetItems, "Id", "Name", transaction.BudgetItemId);
            ViewBag.EnteredById = new SelectList(db.Users, "Id", "FirstName", transaction.EnteredById);
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,AccountId,Description,Date,Amount,Type,EnteredById,Reconciled,ReconciledAmount,BudgetItemId")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(transaction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AccountId = new SelectList(db.Accounts, "Id", "Name", transaction.AccountId);
            ViewBag.BudgetItemId = new SelectList(db.BudgetItems, "Id", "Name", transaction.BudgetItemId);
            ViewBag.EnteredById = new SelectList(db.Users, "Id", "FirstName", transaction.EnteredById);
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Transaction transaction = db.Transactions.Find(id);
            db.Transactions.Remove(transaction);
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
