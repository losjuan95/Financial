using Financial.Models;
using Financial.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Financial.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

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
    }
}