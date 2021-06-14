using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AceTC.Models;
using AceTC.Controllers;

namespace AceTC.Controllers
{
    public class AddParentController : Controller
    {
        // GET: AddParent
        public ActionResult AddParent(int id = 0)
        {
            Parent addp = new Parent();
            return View(addp);
        }

        [HttpPost]
        public ActionResult AddParent(Parent parent)
        {
            using (AceDBEntities db = new AceDBEntities() )
            {
                db.Parents.Add(parent);
                db.SaveChanges();
            }
            ModelState.Clear();
            ViewBag.SuccessMessage = "Registration Successful. ";
            return RedirectToAction("ParentList", "Admin");
        }


    }
}