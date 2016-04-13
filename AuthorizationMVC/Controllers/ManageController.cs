using AuthorizationMVC.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AuthorizationMVC.Controllers
{
    public class ManageController : Controller
    {
        // GET: Manage
        [Authorize(Roles = "admin, user")]
        public ActionResult Index()
        {
            UpdateModel model = new UpdateModel();
            string Email = User.Identity.GetUserName();
            model = ApplicationDbContext.GetInfoUserByEmail(Email);
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "admin, user")]
        [ValidateAntiForgeryToken]
        public ActionResult Index(UpdateModel model)
        {
            if(ModelState.IsValid)
            {
                string email = User.Identity.GetUserName();
                if (ApplicationDbContext.UpdateUserInfoInDb(model, email))
                {
                    return RedirectToAction("Index", "Users", new { username = model.Name, message = "information about user updated" });
                }
                else
                {
                    return RedirectToAction("Index", "Users", new { username = model.Name, message = "error updated information" });
                }
            }
            return View(model);
        }
    }
}