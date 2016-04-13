using AuthorizationMVC.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AuthorizationMVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Reports()
        {
            ViewBag.Message = "Reports";

            return View();
        }

        [HttpGet]
        public JsonResult GetPositionsDepartment()
        {
            string email = User.Identity.GetUserName();
            var j = ApplicationDbContext.ReportGetPositionsDepartment(email);
            return Json(j, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetEmployeesDepartment()
        {
            string email = User.Identity.GetUserName();
            var j = ApplicationDbContext.ReportGetEmplyeesDepartment(email);
            return Json(j, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}