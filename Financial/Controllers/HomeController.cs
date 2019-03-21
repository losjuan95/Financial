using Financial.Helper;
using Financial.Models;
using Financial.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Financial.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = "HOH, Member")]
        public ActionResult Index()
        {
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