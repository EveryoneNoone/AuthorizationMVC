using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AuthorizationMVC.Controllers
{
    public class UsersController : Controller
    {
        // GET: Users
        public ActionResult Index(string username, string message)
        {
            ViewBag.Name = username;
            ViewBag.Message = message;
            return View();
        }
    }
}