using AuthorizationMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AuthorizationMVC.Controllers
{
    public class DepartmentController : Controller
    {
        // GET: Department
        [Authorize(Roles = "admin")]
        public ActionResult Department()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Department(DepartmentModel model)
        {
            if (ModelState.IsValid)
            {
                if (!ApplicationDbContext.CheckDepartmentInDb(model.Name))
                {
                    if (ApplicationDbContext.SetDepartmentToDb(model.Name))
                    {
                        return RedirectToAction("Index", "Users", new { username = model.Name, message = "added successfully" });
                    }
                    else
                    {
                        return RedirectToAction("Index", "Users", new { username = model.Name, message = "add error" });
                    }
                }
                else
                {
                    return RedirectToAction("Index", "Users", new { username = model.Name, message = "The department exists" });
                }

            }
            return View(model);
        }

        [Authorize(Roles = "admin")]
        public ActionResult Position()
        {
            ViewBag.Departments = new SelectList(ApplicationDbContext.GetDepartmentListFromDb(), "IdDepartment", "Name");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Position(PositionModel model)
        {
            if (ModelState.IsValid)
            {
                if(!ApplicationDbContext.CheckPositionInDb(model))
                {
                    if(ApplicationDbContext.SetPositionToDb(model))
                    {
                        return RedirectToAction("Index", "Users", new { username = model.Name, message = "added successfully" });
                    }
                    else
                    {
                        return RedirectToAction("Index", "Users", new { username = model.Name, message = "add error" });
                    }
                }
                else
                {
                    return RedirectToAction("Index", "Users", new { username = model.Name, message = "The position exists" });
                }
            }
            return View();
        }
    }
}