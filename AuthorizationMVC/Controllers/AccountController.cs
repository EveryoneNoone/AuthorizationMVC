using AuthorizationMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AuthorizationMVC.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        // GET: Account
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = ApplicationDbContext.CheckLoginInDb(model.Email, model.Password);

            if (!string.IsNullOrEmpty(user.Name))
            {

                FormsAuthentication.SetAuthCookie(model.Email, true);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "The username or password is not correct");
            }
            return View(model);
        }

        [Authorize(Roles = "admin")]
        public ActionResult Register()
        {
            ViewBag.Name = new SelectList(ApplicationDbContext.GetRolesListFromDb(), "IdRole", "Role");
            ViewBag.Departments = new SelectList(ApplicationDbContext.GetDepartmentListFromDb(), "IdDepartment", "Name");                       
            return View();
        }

        [HttpGet]
        public JsonResult GetPositionsDepartmentList(string IdDepartment)
        {
            SelectList j = new SelectList(new List<Positions>(), "IdPosition", "Name");            
            if (!string.IsNullOrEmpty(IdDepartment))
            {
                j = new SelectList(ApplicationDbContext.GetPositionsDepartmentFromDb(IdDepartment), "IdPosition", "Name");
            }
            return Json(j, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                if (ApplicationDbContext.CheckEmailInDb(model.Email))
                {
                    return RedirectToAction("Index", "Users", new { username = "", message = "The username with this email already exists" });
                }
                else
                {
                    if (ApplicationDbContext.RegisterUserInDb(model))
                    {
                        return RedirectToAction("Index", "Users", new { username = model.Name, message = "added successfully" });
                    }
                    else
                    {
                        return RedirectToAction("Index", "Users", new { username = model.Name, message = "error add" });
                    }
                }
            }
            return RedirectToAction("Index", "Users", new { username = model.Name, message = "error add" }); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}