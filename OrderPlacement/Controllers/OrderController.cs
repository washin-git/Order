using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OrderPlacement.Models;

namespace OrderPlacement.Controllers
{
    public class OrderController : Controller
    {
        [HttpGet]
        // GET: Order
        public ActionResult Index(int id=0)
        {
            User usermodel = new User();
            return View(usermodel);
        }

        [HttpPost]
        public ActionResult Index(User usermodel)
        {
        
            using(orderplacementEntities2 dbmodel = new orderplacementEntities2())
            {
                if(dbmodel.Users.Any(x => x.UserName == usermodel.UserName))
                {
                    ViewBag.DuplicateMessage = "User Name already exist";
                    return View("Index", usermodel);
                }
                else if (dbmodel.Users.Any(x => x.Email == usermodel.Email))
                {
                    ViewBag.DuplicateMessage = "Email address already tacken";
                    return View("Index", usermodel);
                }
                dbmodel.Users.Add(usermodel);
                dbmodel.SaveChanges();
            }
            ModelState.Clear();
            ViewBag.SuccessMessage = "Successfully Ordered";
            ViewBag.SuccessMessage2 = "Now you can login and view user orders";
            return View("Index", new User());
            
        }

    }
}