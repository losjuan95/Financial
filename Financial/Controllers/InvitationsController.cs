﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Financial.Helper;
using Financial.Models;
using Microsoft.AspNet.Identity;
using System.Net.Mail;
using System.Web.Configuration;

namespace Financial.Controllers
{
    public class InvitationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public static AuthorizeHelper Authorize = new AuthorizeHelper();

        
        // GET: Invitations
        public ActionResult Index()
        {
            var invitations = db.Invitations.Include(i => i.HouseHold);
            return View(invitations.ToList());
        }

        // GET: Invitations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invitation invitation = db.Invitations.Find(id);
            if (invitation == null)
            {
                return HttpNotFound();
            }
            return View(invitation);
        }

        // GET: Invitations/Create
        public ActionResult Create()
        {
            ViewBag.HouseHoldId = new SelectList(db.HouseHolds, "Id", "Description");
            return View();
        }

        // POST: Invitations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <ActionResult> Create([Bind(Include = "Id,Body,Email")] Invitation invitation)
        {
            
         
            if (ModelState.IsValid)
            {

                var usershouse = db.Users.Find(User.Identity.GetUserId()).HouseHoldId.GetValueOrDefault();
                invitation.Created = DateTime.Now;
                invitation.Expires = DateTime.Now.AddDays(1);
                invitation.Expired = true;
                invitation.KeyCode = Guid.NewGuid();
                invitation.HouseHoldId = usershouse;
                var from = $"FinPort<{WebConfigurationManager.AppSettings["emailfrom"]}>";
                var callbackUrl = Url.Action("AcceptRegister", "Account", new { KeyCode = invitation.KeyCode, Email = invitation.Email}, protocol: Request.Url.Scheme);

                var message = new MailMessage(from, invitation.Email)
                {
                    Subject = "FinPort Invite",
                    Body = "<a href=\"" + callbackUrl + "\">click</a>",
                    IsBodyHtml = true
                    
                };

                var emailService = new PersonalEmail();
                await emailService.SendAsync(message);

                db.Invitations.Add(invitation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        
               
          

            ViewBag.HouseHoldId = new SelectList(db.HouseHolds, "Id", "Description", invitation.HouseHoldId);
            //await AuthorizeHelper.ReauthorizeUserAsync(userId);

            return View(invitation);

            
        }

        // GET: Invitations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invitation invitation = db.Invitations.Find(id);
            if (invitation == null)
            {
                return HttpNotFound();
            }
            ViewBag.HouseHoldId = new SelectList(db.HouseHolds, "Id", "Description", invitation.HouseHoldId);
            return View(invitation);
        }

        // POST: Invitations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,HouseHoldId,Body,KeyCode,Email,Created,Accepted,Expires,Expired")] Invitation invitation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(invitation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HouseHoldId = new SelectList(db.HouseHolds, "Id", "Description", invitation.HouseHoldId);
            return View(invitation);
        }

        // GET: Invitations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invitation invitation = db.Invitations.Find(id);
            if (invitation == null)
            {
                return HttpNotFound();
            }
            return View(invitation);
        }

        // POST: Invitations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Invitation invitation = db.Invitations.Find(id);
            db.Invitations.Remove(invitation);
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
