using AceTC.Models;
using AceTC.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;


namespace AceTC.Controllers
{
    public class ParentController : Controller
    {
        // GET: Parent
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult ViewChildren()
        {
            AceDBEntities entity = new AceDBEntities();
            string uid = Session["parents_ic"].ToString();
            List<Student> studentpackage = entity.Students.Where(a => a.parent_ic.Equals(uid)).ToList();
            List<Package> packagename = entity.Packages.ToList();
            var jointable = from s in studentpackage
                            join p in packagename on s.student_package equals p.package_id into table1
                            from p in table1.DefaultIfEmpty()
                            select new JoinClass { studentdetails = s, packagedetails = p };

            // var list = entity.Students.Where(a => a.parent_ic.Equals(uid));
            //return View(entity.Students.ToList());
            return View(jointable);
        }

        public ActionResult ViewChildrenDetails(string id)
        {
            AceDBEntities entity = new AceDBEntities();
            //Student stud = entity.Students.Find(id);
            List<Student> studentpackage = entity.Students.Where(a => a.student_ic.Equals(id)).ToList();
            List<Package> packagename = entity.Packages.ToList();
            List<studRegister> subjlist = entity.studRegisters.ToList();

            var combinetable = from s in studentpackage
                               join p in packagename on s.student_package equals p.package_id into table1
                               from p in table1.DefaultIfEmpty()
                               join o in subjlist on s.student_ic equals o.studreg_ic
                               select new JoinClass { studentdetails = s, packagedetails = p , studsubjdetails = o};


            return View(combinetable);

        }

        public ActionResult MakePayment(string id)
        {
            AceDBEntities entity = new AceDBEntities();



            List<Parent> parentlist = entity.Parents.ToList();
            List<Payment> paymentdetails = entity.Payments.ToList();
            List<Student> studentlist = entity.Students.ToList();
            List<Status> statusdetails = entity.Status.ToList();

            var multipletable = from pt in paymentdetails
                                join st in statusdetails on pt.status_id equals st.status_id
                                join s in studentlist on pt.student_ic equals s.student_ic
                                join par in parentlist on pt.parent_ic equals par.parents_ic
                                where pt.parent_ic == Session["parents_ic"].ToString()
                                select new MultipleClass { statusdetails = st, paymentdetails = pt, parentdetails = par, studentdetails = s };


            return View(multipletable);
        }

        public ActionResult ViewPaymentHistory()
        {
            AceDBEntities entity = new AceDBEntities();
            string uid = Session["parents_ic"].ToString();
            List<Student> studentlist = entity.Students.ToList();
            List<Parent> parentlist = entity.Parents.ToList();
            List<Status> statuslist = entity.Status.ToList();
            List<Package> packagelist = entity.Packages.ToList();
            List<studRegister> register = entity.studRegisters.ToList();
            List<Payment> paymentlist = entity.Payments.Where(a => a.parent_ic.Equals(uid)).ToList();

            var multipletable = from pa in paymentlist
                                join p in parentlist on pa.parent_ic equals p.parents_ic
                                join s in studentlist on pa.student_ic equals s.student_ic
                                join st in statuslist on pa.status_id equals st.status_id
                                join reg in register on pa.student_ic equals reg.studreg_ic
                                /*                                join pack in packagelist on pa.package_id equals pack.package_id*/
                                select new MultipleClass { studentdetails = s, parentdetails = p, paymentdetails = pa, statusdetails = st, studentregister = reg };


            return View(multipletable);
        }

        public ActionResult ViewSubject()
        {
            AceDBEntities slist = new AceDBEntities();
            return View(from Subject in slist.Subjects select Subject);

        }

        // GET: editpass/Edit/5
        public ActionResult editPassword(string id)
        {
            string uid = Session["parents_ic"].ToString();
            AceDBEntities entity = new AceDBEntities();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parent parent = entity.Parents.Find(id);
            if (parent == null)
            {
                return HttpNotFound();
            }
            return View(parent);
        }

        // POST: editpass/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult editPassword([Bind(Include = "parents_ic,parents_name,parents_pass,confirmPass,parents_email,parents_phone,parents_address")] Parent parent)
        {
            string uid = Session["parents_ic"].ToString();
            AceDBEntities entity = new AceDBEntities();
            if (ModelState.IsValid)
            {
                entity.Entry(parent).State = EntityState.Modified;
                entity.SaveChanges();
                return RedirectToAction("ViewChildren", "Parent");
            }
            return View(parent);

        }


        [HttpGet]
        public ActionResult Upload(int id)
        {
            AceDBEntities entity = new AceDBEntities();
            Payment p = entity.Payments.Find(id);
            if (p == null)
            {
                return HttpNotFound();
            }

            return View(p);
        }
        [HttpPost]
        public ActionResult Upload(int? id, ParentViewModel pvm)
        {
            using (AceDBEntities db = new AceDBEntities())
            {
                //string guid = Guid.NewGuid().ToString();
                //string filepath = guid + Path.GetExtension(pvm.filename.FileName);
                string filepath = Path.GetFileNameWithoutExtension(pvm.filename.FileName);
                string fileextension = Path.GetExtension(pvm.filename.FileName);
                filepath = DateTime.Now.ToString("ddMMyyyy") + "-" + filepath.Trim() + fileextension;
                Payment p = db.Payments.Find(id);


                pvm.filename.SaveAs(Server.MapPath("~/upload/" + filepath));
                p.filename = "~/upload/" + filepath;
                p.status_id = 4;
                p.payment_date = DateTime.Now;
                db.Entry(p).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ViewPaymentHistory", "Parent");
            }

        }

    }

}