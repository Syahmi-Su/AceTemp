using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AceTC.Models;
using AceTC.Controllers;
namespace AceTC.Controllers
{
    public class AddStudentController : Controller
    {
        AceDBEntities db = new AceDBEntities();
        // GET: AddStudent
        public ActionResult AddStudent()
        {
            Student addstud = new Student();
            var par = db.Parents.ToList();
            var pack = db.Packages.ToList();
            if(par!=null&&pack!=null)
            {
                ViewBag.data = par;
                ViewBag.packs = pack;
            }
            return View(addstud);
        }

        [HttpPost]
        public ActionResult AddStudent(Student student)
        {
            using (AceDBEntities db = new AceDBEntities())
            {
                db.Students.Add(student);
                db.SaveChanges();
            }
            ModelState.Clear();
            ViewBag.SuccessMessage = "Registration Successful. ";
            return RedirectToAction("StudentList", "Admin");
        }


    }
}