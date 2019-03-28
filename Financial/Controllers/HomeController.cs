﻿using Financial.Helper;
using Financial.Models;
using Financial.ViewModels;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Financial.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        UserRolesHelpers roleHelper = new UserRolesHelpers();
        BudgetHelper bHelper = new BudgetHelper();

        [Authorize(Roles = "HOH, Member")]
        public ActionResult Index()
        {
            List<DataPoints> dataPoints = new List<DataPoints>();

            dataPoints.Add(new DataPoints("Utilities", 1));
            dataPoints.Add(new DataPoints("Food", 2));
            dataPoints.Add(new DataPoints("Insurance", 4));
            dataPoints.Add(new DataPoints("Housing", 4));
            dataPoints.Add(new DataPoints("Personal", 9));
            dataPoints.Add(new DataPoints("Savings", 11));
            dataPoints.Add(new DataPoints("Debt", 13));

            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);

            return View();
        }

        public ActionResult Lobby()
        {
           
            var guests = db.Users.Where(u => u.HouseHoldId == null)
                            .Select(guestuser => new GuestUser
                            {
                                Name = guestuser.FirstName + "" + guestuser.LastName,
                                Email = guestuser.Email,
                                PhoneNumber = guestuser.PhoneNumber,
                                AvatarPath = ""
                            }).ToList();
                    
            return View(guests);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]    
        public async Task<ActionResult> LeaveHouse()
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);

            user.HouseHoldId = null;
            db.SaveChanges();
            
            roleHelper.RemoveUserFromRole(userId, "Member");
           await AuthorizeHelper.ReauthorizeUserAsync(userId);

            return RedirectToAction("Lobby");
        }
        public ActionResult LandingPage()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeAvatar(HttpPostedFileBase image)
        {

            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            if (FileUploadValidator.AvatarUploadValidator(image))
            {

                var imageName = Path.GetFileName(image.FileName);
                image.SaveAs(Path.Combine(Server.MapPath("~/Uploads/"), imageName));
                user.AvatarPath = "/Uploads/" + imageName;
                db.Users.Attach(user);
                db.Entry(user).Property(u => u.AvatarPath).IsModified = true;
                db.SaveChanges();

                return RedirectToAction("Index", "Manage");

            }
            else
            {
                return RedirectToAction("Index", "Manage");
            }


        }
    }
}