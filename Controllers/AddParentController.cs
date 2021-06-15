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
        AceDBEntities db = new AceDBEntities();

        // GET: AddParent
        public ActionResult AddParent(int id = 0)
        {
            Parent addp = new Parent();
            return View(addp);
        }


        [HttpPost]
        public ActionResult AddParent(Parent parent)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    db.Parents.Add(parent);
                    db.SaveChanges();

                    ViewBag.msg = "Parents created successfully.";

                    return RedirectToAction("ParentList", "Admin");
                }
                else
                {

                    return View();
                }

            }
            catch (Exception)
            {
                ViewBag.Error = "Parents with IC number already exist.";

                return View();

            }

        }

    }
}