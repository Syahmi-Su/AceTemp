using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AceTC.Models;

namespace AceTC.Controllers
{
    public class AddSubjectController : Controller
    {
        // GET: AddSubject
        public ActionResult AddSubject()
        {
            Subject addsub = new Subject();
            return View(addsub);
        }

        [HttpPost]
        public ActionResult AddSubject(Subject subject)
        {
            using (AceDBEntities db = new AceDBEntities())
            {
                db.Subjects.Add(subject);
                db.SaveChanges();
            }
            ModelState.Clear();
            ViewBag.SuccessMessage = "Add Subject Successfully. ";
            return RedirectToAction("SubjectList", "Admin");
        }
    }
}