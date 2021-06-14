using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using AceTC.Models;

namespace AceTC.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AdminDashboard()
        {
            AceDBEntities entity = new AceDBEntities();
            CountClass cnt = new CountClass()
            {
                studentcount = entity.Students.Count(),
                packagecount = entity.Packages.Count(),
                paymentcount = entity.Payments.Count(),
                outstandingcount = entity.Outstandings.Count(),
                subjectcount = entity.Subjects.Count(),
                
            };
            return View(cnt);

            //var x = entity.Students.Include("Payment").ToList().GroupBy(e => e.student_category).Select(y => new Count
            //{
            //    category = y.First().student_category,
            //    count = y.Count()
            //}).ToList();

            //return View(x);
        }

        // GET: STUDENT
        public ActionResult StudentList()
        {
            AceDBEntities entity = new AceDBEntities();
            List<Student> studentparent = entity.Students.ToList();
            List<Parent> parentname = entity.Parents.ToList();
            List<Package> packagename = entity.Packages.ToList();

            var multipletable = from s in studentparent
                                join p in parentname on s.parent_ic equals p.parents_ic 
                                join pc in packagename on s.student_package equals pc.package_id
                                select new MultipleClass { studentdetails = s, parentdetails = p, packagedetails = pc };

            return View(multipletable);
        }

        public ActionResult EditStudentDetails(string id)
        {
            AceDBEntities entity = new AceDBEntities();
            Student stud = entity.Students.Find(id);
            var par = entity.Parents.ToList();
            var pack = entity.Packages.ToList();
            if (par != null && pack != null)
            {
                ViewBag.data = par;
                ViewBag.packs = pack;
            }

                return View(stud);
        }

        [HttpPost]
        public ActionResult EditStudentDetails(Student student)
        {
            /*AceDBEntities entity = new AceDBEntities();
            entity.Entry(student).State = EntityState.Modified;
            entity.SaveChanges();
            //ModelState.Clear();
            //ViewBag.SuccessMessage = "Save Changes Successful. ";
            return RedirectToAction("StudentList", "Admin");*/

            using (AceDBEntities entity = new AceDBEntities())
            {
                entity.Entry(student).State = EntityState.Modified;
                entity.SaveChanges();
            }
            ModelState.Clear();
            //ViewBag.SuccessMessage = "Save Changes Successful. ";
            return RedirectToAction("StudentList", "Admin");

        }

        public ActionResult DeleteStudent(string id)
        {
            AceDBEntities entity = new AceDBEntities();
            Student std = entity.Students.Find(id);
            if (std == null)
                return View("NotFound");
            else
                return View(std);
        }

        [HttpPost]
        [ActionName("DeleteStudent")]
        public ActionResult ConfirmedDeleteStudent(string id)
        {
            AceDBEntities entity = new AceDBEntities();
            Student std = entity.Students.Find(id);
            entity.Students.Remove(std);
            entity.SaveChanges();
            return RedirectToAction("StudentList", "Admin");
        }



        // GET: PARENT
        public ActionResult ParentList()
        {
            AceDBEntities plist = new AceDBEntities();
            return View(from Parent in plist.Parents select Parent);
        }

        public ActionResult EditParentDetails(string id)
        {
            AceDBEntities entity = new AceDBEntities();
            Parent prnt = entity.Parents.Find(id);
            if (prnt == null)
                return View("NotFound");
            else
                return View(prnt);
        }
        [HttpPost]
        public ActionResult EditParentDetails(Parent parent)
        {
            using (AceDBEntities entity = new AceDBEntities())
            {
                entity.Entry(parent).State = EntityState.Modified;
                entity.SaveChanges();
            }
            ModelState.Clear();
            //ViewBag.SuccessMessage = "Save Changes Successful. ";
            return RedirectToAction("ParentList", "Admin");

        }



        public ActionResult DeleteParent(string id)
        {
            AceDBEntities entity = new AceDBEntities();
            Parent prnt = entity.Parents.Find(id);
            if (prnt == null)
                return View("NotFound");
            else
                return View(prnt);
        }
        [HttpPost]
        [ActionName("DeleteParent")]
        public ActionResult ConfirmedDeleteParent(string id)
        {
            AceDBEntities entity = new AceDBEntities();
            Parent prnt = entity.Parents.Find(id);
            entity.Parents.Remove(prnt);
            entity.SaveChanges();
            return RedirectToAction("ParentList", "Admin");
        }


        // GET: PACKAGE
        public ActionResult PackageList()
        {
            AceDBEntities entity = new AceDBEntities();
            List<Student> studentparent = entity.Students.ToList();
            List<Package> packagename = entity.Packages.ToList();

            var multable = from pc in packagename
                                join s in studentparent on pc.package_id equals s.student_package
                                select new MultipleClass { studentdetails = s, packagedetails = pc };
            return View(multable);

        }


        public ActionResult EditPackageDetails(int id)
        {

            AceDBEntities entity = new AceDBEntities();
            Package pack = entity.Packages.Find(id);
            if (pack == null)
                return View("NotFound");
            else
                return View(pack);
        }
        [HttpPost]
        public ActionResult EditPackageDetails(Package package)
        {
            /*AceDBEntities entity = new AceDBEntities();
            entity.Entry(student).State = EntityState.Modified;
            entity.SaveChanges();
            //ModelState.Clear();
            //ViewBag.SuccessMessage = "Save Changes Successful. ";
            return RedirectToAction("StudentList", "Admin");*/

            using (AceDBEntities entity = new AceDBEntities())
            {
                entity.Entry(package).State = EntityState.Modified;
                entity.SaveChanges();
            }
            ModelState.Clear();
            ViewBag.SuccessMessage = "Save Changes Successful. ";
            return RedirectToAction("PackageList", "Admin");

        }

        public ActionResult SubjectList()
        {
            AceDBEntities slist = new AceDBEntities();
            return View(from Subject in slist.Subjects select Subject);
    
        }


        
       

        public ActionResult DeletePackage(int id)
        {
            AceDBEntities entity = new AceDBEntities();
            Package pkg = entity.Packages.Find(id);
            if (pkg == null)
                return View("NotFound");
            else
                return View(pkg);
        }

        [HttpPost]
        [ActionName("DeletePackage")]
        public ActionResult ConfirmedDeletePackage(int id)
        {
            AceDBEntities entity = new AceDBEntities();
            Package pkg = entity.Packages.Find(id);
            entity.Packages.Remove(pkg);
            entity.SaveChanges();
            return RedirectToAction("PackageList", "Admin");
        }


        public ActionResult EditSubjectDetails(string id)
        {
            AceDBEntities entity = new AceDBEntities();
            Subject subj = entity.Subjects.Find(id);
            if (subj == null)
                return View("NotFound");
            else
                return View(subj);
        }

        [HttpPost]
        public ActionResult EditSubjectDetails(Subject subject)
        {
            using (AceDBEntities entity = new AceDBEntities())
            {
                entity.Entry(subject).State = EntityState.Modified;
                entity.SaveChanges();
            }
            ModelState.Clear();
            //ViewBag.SuccessMessage = "Save Changes Successful. ";
            return RedirectToAction("SubjectList", "Admin");

        }

        public ActionResult DeleteSubject(string id)
        {
            AceDBEntities entity = new AceDBEntities();
            Subject subj = entity.Subjects.Find(id);
            if (subj == null)
                return View("NotFound");
            else
                return View(subj);
        }
        
        [HttpPost]
        [ActionName("DeleteSubject")]
        public ActionResult ConfirmedDeleteSubject(string id)
        {
            AceDBEntities entity = new AceDBEntities();
            Subject subject = entity.Subjects.Find(id);
            entity.Subjects.Remove(subject);
            entity.SaveChanges();
            return RedirectToAction("SubjectList", "Admin");
        }


        public ActionResult PaymentHistory()
        {
            return View();
        }

        public ActionResult testing()
        {
            return View();
        }
    }
}