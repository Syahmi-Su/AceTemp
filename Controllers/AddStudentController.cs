using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AceTC.Models;
using AceTC.Controllers;
using System.Data.Entity;

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
            var studentIC = student.student_ic;
            var studPackage = student.student_package;
            var cat = student.student_category;

            Package p = db.Packages.Find(studPackage);
            var totalsubj = p.total_subject;

            Payment pay = new Payment();
            pay.student_ic = student.student_ic;
            pay.parent_ic = student.parent_ic;
            pay.payment_fee = p.package_price;
            pay.ref_num = "ACE" + pay.confirmation_id;
            pay.status_id = 1;
            pay.confirmation_date = DateTime.Now;
            pay.payment_date = DateTime.Now;

            string p_details = p.package_category + "/ " + p.package_desc;
            pay.payment_detail = p_details;

            Outstanding outs = new Outstanding();
            DateTime nextMonth = DateTime.Now;
            outs.O_month = nextMonth;
            outs.O_pID = student.parent_ic;
            outs.O_fees = p.package_price;
            outs.O_remark = "Monthly Payment";
            outs.O_status = 1;
            outs.O_stu = studentIC;

            using (AceDBEntities db = new AceDBEntities())
            {
                db.Payments.Add(pay);
                db.Students.Add(student);
                db.Outstandings.Add(outs);
                db.SaveChanges();
            }
            ModelState.Clear();

            return RedirectToAction("AddStudentSubject", new { studentIC, totalsubj, cat});     
        }
            
        public ActionResult AddStudentSubject(string studentIC, int totalsubj, string cat)
        {
            ViewBag.totalsub = totalsubj;
            var subj = db.Subjects.Where(x => x.subject_type == cat).ToList(); ViewBag.subj = subj;
            studRegister std = new studRegister();
            std.studreg_ic = studentIC;

            return View(std);
        }

        [HttpPost]
        public ActionResult AddStudentSubject(string ic, studRegister reg)
        {
            if(ModelState.IsValid)
            {
                reg.subject_1 = reg.subject_1;
                reg.subject_2 = reg.subject_2;
                reg.subject_3 = reg.subject_3;
                reg.subject_4 = reg.subject_4;
                reg.subject_5 = reg.subject_5;
                reg.subject_6 = reg.subject_6;
                reg.subject_7 = reg.subject_7;
                db.studRegisters.Add(reg);
                db.SaveChanges();
            }

            return RedirectToAction("StudentList", "Admin");
        }


    }
}