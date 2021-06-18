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
        private AceDBEntities db = new AceDBEntities();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AdminDashboard()
        {

            using (AceDBEntities entity = new AceDBEntities())
            {
                int totstud = (from tot in entity.Students select tot.student_ic).Count();
                int primarystud = (from tot in entity.Students where tot.student_category == "Primary" select tot.student_ic).Count();
                int secondarystud = (from tot in entity.Students where tot.student_category == "Secondary" select tot.student_ic).Count();
                int totsubj = (from tot in entity.Outstandings select tot.O_ID).Count();
                var totpack = entity.Payments.Where(x => x.status_id == 1 || x.status_id == 7).Sum(y => y.payment_fee);
                int totouts = (from tot in entity.Payments where (tot.status_id == 2) select tot.confirmation_id).Count();
                int pendpay = (from tot in entity.Payments where (tot.status_id == 1||tot.status_id == 4) select tot.confirmation_id).Count();

                var ppaytotal = entity.Payments.Where(x => x.status_id == 7 || x.status_id == 2).Sum(y => y.payment_fee);
                

                ViewData["totalstudents"] = totstud;
                var totalstudents = ViewData["totalstudents"];

                ViewData["primarystudents"] = primarystud;
                var primarystudents = ViewData["primarystudents"];

                ViewData["secondarystudents"] = secondarystud;
                var secondarystudents = ViewData["secondarystudents"];

                ViewData["totalsubjects"] = totsubj;
                var totalsubjects = ViewData["totalsubjects"];

                ViewData["totalpackages"] = totpack;
                var totalpackages = ViewData["totalpackages"];

                ViewData["totaloutstandings"] = totouts;
                var totaloutstandings = ViewData["totaloutstandings"];

                ViewData["payment"] = ppaytotal;
               var pendingpayment = ViewData["payment"];

                List<Outstanding> outs = entity.Outstandings.ToList();
                List<Status> stats = entity.Status.ToList();

                var stattable = from o in outs
                                join s in stats on o.O_status equals s.status_id
                                select new MultipleClass { statusdetails = s, outstandingdetails = o };

                return View(stattable);

            }


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
            var studentIC = student.student_ic;
            var studPackage = student.student_package;
            var cat = student.student_category;

            Package p = db.Packages.Find(studPackage);
            var totalsubj = p.total_subject;
            var fee = p.package_price;

            Payment pay = db.Payments.Where(x => x.student_ic == student.student_ic).FirstOrDefault();
            pay.payment_fee =fee ;
            pay.ref_num = "ACE";
            pay.status_id = 1;
            pay.confirmation_date = DateTime.Now;
            pay.payment_date = DateTime.Now;
            string p_details = p.package_category + "/ " + p.package_desc;

            using (AceDBEntities entity = new AceDBEntities())
            {
                entity.Entry(pay).State = EntityState.Modified;
                entity.Entry(student).State = EntityState.Modified;
                entity.SaveChanges();
            }
            ModelState.Clear();
            //ViewBag.SuccessMessage = "Save Changes Successful. ";
            return RedirectToAction("editStudentList", new { studentIC , totalsubj, cat});

            }

        public ActionResult editStudentList(string studentIC, int totalsubj, string cat)
        {
            ViewBag.totalsub = totalsubj;
            var subj = db.Subjects.Where(x => x.subject_type == cat).ToList();
            ViewBag.subj = subj;

            studRegister std = db.studRegisters.Find(studentIC);

            return View(std);
        }

        [HttpPost]
        public ActionResult editStudentList(string ic, studRegister reg)
        {
            if (ModelState.IsValid)
            {
                reg.subject_1 = reg.subject_1;
                reg.subject_2 = reg.subject_2;
                reg.subject_3 = reg.subject_3;
                reg.subject_4 = reg.subject_4;
                reg.subject_5 = reg.subject_5;
                reg.subject_6 = reg.subject_6;
                reg.subject_7 = reg.subject_7;
                db.Entry(reg).State = EntityState.Modified;
                db.SaveChanges();
            }

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
            return View(from Package in entity.Packages select Package);

        }

        public ActionResult SubjectList()
        {
            AceDBEntities slist = new AceDBEntities();
            return View(from Subject in slist.Subjects select Subject);

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

            using (AceDBEntities entity = new AceDBEntities())
            {
                entity.Entry(package).State = EntityState.Modified;
                entity.SaveChanges();
            }
            ModelState.Clear();
            ViewBag.SuccessMessage = "Save Changes Successful. ";
            return RedirectToAction("PackageList", "Admin");

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