using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Financial.Enumerations;
using Financial.Helper;
using Financial.Models;
using Microsoft.AspNet.Identity;

namespace Financial.Controllers
{
    public class HouseHoldsController : Controller
    {
        UserRolesHelpers RolesHelpers = new UserRolesHelpers();
        private ApplicationDbContext db = new ApplicationDbContext();
        AuthorizeHelper Authorize = new AuthorizeHelper();


        // GET: HouseHolds
        [Authorize(Roles = "HOH, Member")]

        public ActionResult Index()
        {
            return View(db.HouseHolds.ToList());
        }

        [Authorize(Roles = "HOH, Member")]
        public ActionResult DashBoard()
        {
            var userId = User.Identity.GetUserId();
            var householdid = db.Users.Find(userId).HouseHoldId;
            var house = db.HouseHolds.Find(householdid);

            

            return View(house);
        }

        // GET: HouseHolds/Details/5
        [Authorize(Roles = "HOH, Member")]

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HouseHold houseHold = db.HouseHolds.Find(id);
            if (houseHold == null)
            {
                return HttpNotFound();
            }
            return View(houseHold);
        }

        // GET: HouseHolds/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HouseHolds/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Description,Name,Greeting")] HouseHold houseHold)
        {
            if (ModelState.IsValid)
            {
                db.HouseHolds.Add(houseHold);
                db.SaveChanges();

                var userId = User.Identity.GetUserId();
                var user = db.Users.Find(userId);
                user.HouseHoldId = houseHold.Id;
                //db.Entry(user.Id).Property;

                db.SaveChanges();

                RolesHelpers.AddUserToRole(userId, RoleName.HOH);

                await AuthorizeHelper.ReauthorizeUserAsync(userId);

                return RedirectToAction("Dashboard");
            }

           


            return RedirectToAction("Index", "Home");
        }

        // GET: HouseHolds/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HouseHold houseHold = db.HouseHolds.Find(id);
            if (houseHold == null)
            {
                return HttpNotFound();
            }
            return View(houseHold);
        }

        // POST: HouseHolds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Description,Name,Greeting")] HouseHold houseHold)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser applicationUser = db.Users.Find(houseHold);

                var currentroles = RolesHelpers.ListUserRoles(applicationUser.Id);
                foreach (var role in currentroles)
                {
                    RolesHelpers.RemoveUserFromRole(applicationUser.Id, role);

                }
                //if (!string.IsNullOrEmpty(houseHold.ToString()))
                //{
                //    RolesHelpers.AddUserToRole(applicationUser.Id, houseHold.ToString());
                //}

                db.Entry(houseHold).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(houseHold);
        }

        // GET: HouseHolds/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HouseHold houseHold = db.HouseHolds.Find(id);
            if (houseHold == null)
            {
                return HttpNotFound();
            }
            return View(houseHold);
        }

        // POST: HouseHolds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HouseHold houseHold = db.HouseHolds.Find(id);
            db.HouseHolds.Remove(houseHold);
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
