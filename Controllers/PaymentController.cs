using AceTC.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace AceTC.Controllers
{
    public class PaymentController : Controller
    {
        private AceDBEntities db = new AceDBEntities();

        // GET: Payment
        public ActionResult Index()
        {
            return View(db.Payments.ToList());
        }

        // GET: Payment/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment payment = db.Payments.Find(id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            return View(payment);
        }

        // GET: Payment/Create
        public ActionResult AddPayment()
        {
            Payment addpack = new Payment();
            var par = db.Parents.ToList();
            var pack = db.Packages.ToList();
            if (par != null && pack != null)
            {
                ViewBag.data = par;
                ViewBag.packs = pack;
            }
            var Id = db.Payments.OrderByDescending(c => c.confirmation_id).FirstOrDefault();

            if (Id == null)
            {
                addpack.confirmation_id = 1;
            }
            else
            {
                addpack.confirmation_id = Id.confirmation_id + 1;
            }
            

            List<SelectListItem> a = new List<SelectListItem>()
            {
                new SelectListItem {
                    Text = "RM20/Month", Value = "20"
                },
                new SelectListItem {
                    Text = "RM40/Month", Value = "40"
                },
                new SelectListItem {
                    Text = "RM60/Month", Value = "60"
                },
                new SelectListItem {
                    Text = "RM80/Month", Value = "80"
                },
                new SelectListItem {
                    Text = "RM100/Month", Value = "100"
                },
            };
            ViewBag.transport_fee = new SelectList(a, "Value", "Text");

            List<SelectListItem> b = new List<SelectListItem>()
            {
                new SelectListItem {
                    Text = "Approved", Value = "2"
                },
                new SelectListItem {
                    Text = "Pending", Value = "1"
                },
            };
            ViewBag.status_id = new SelectList(b, "Value", "Text");


            return View(addpack);
        }

        // POST: Payment/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPayment([Bind(Include = "confirmation_id,student_ic,parent_ic,payment_fee,ref_num,status_id,confirmation_date,payment_date,payment_detail,payment_feedetails,filename, meal_fee,transport_fee,first_register,lower_discount")] Payment p)
        {

            double total = p.payment_fee + p.transport_fee + p.first_register + p.meal_fee;
            double discount = (100 - p.lower_discount) / 100;
            p.payment_fee = total * discount;

            if (ModelState.IsValid)
            {
                db.Payments.Add(p);
                db.SaveChanges();
                ViewBag.SuccessMessage = "Registration Successful. ";
                return RedirectToAction("AdminDashboard", "Admin");
            }

            return View(p);
        }

        // GET: Payment/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment payment = db.Payments.Find(id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            return View(payment);
        }

        // POST: Payment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "confirmation_id,student_ic,parent_ic,payment_fee,ref_num,status_id,confirmation_date,payment_date,payment_detail,payment_feedetailss,filename,meal_fee,transport_fee,first_register,lower_discount")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(payment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(payment);
        }

        public ActionResult MakePayment()
        {
            AceDBEntities entity = new AceDBEntities();
            List<Student> student = entity.Students.ToList();
            List<Parent> parentname = entity.Parents.ToList();
            List<Outstanding> outstanding = entity.Outstandings.ToList();

            var multipletable = from s in student
                                join p in parentname on s.parent_ic equals p.parents_ic into table1
                                from p in table1.DefaultIfEmpty()
                                join o in outstanding on p.parents_ic equals o.O_pID into table2
                                from o in table2.DefaultIfEmpty()
                                select new MultipleClass { studentdetails = s, parentdetails = p, outstandingdetails = o };

            return View(multipletable);

        }

        public ActionResult ApprovalList()
        {
            //AceDBEntities entity = new AceDBEntities();
            //List<Payment> payment = entity.Payments.ToList();
            //List<Status> status = entity.Status.ToList();

            //var multipletable = from a in payment
            //                    join b in status on a.status_id equals b.status_id into table1
            //                    from b in table1.DefaultIfEmpty()
            //                    select new MultipleClass { paymentdetails = a, statusdetails = b};

            //return View(multipletable);

            AceDBEntities entity = new AceDBEntities();
            List<Payment> payment = entity.Payments.ToList();
            List<Status> status = entity.Status.ToList();
            List<Package> package = entity.Packages.ToList();
            List<Student> student = entity.Students.ToList();
            List<Parent> parent = entity.Parents.ToList();

            var multipletable = from a in payment
                                join b in status on a.status_id equals b.status_id
                                join p in package on a.package_id equals p.package_id
                                join s in student on a.student_ic equals s.student_ic
                                join par in parent on a.parent_ic equals par.parents_ic
                                select new MultipleClass { paymentdetails = a, statusdetails = b, packagedetails = p, parentdetails = par, studentdetails = s };

            return View(multipletable);

        }

        public ActionResult Approval(int id)
        {
            var par = db.Status.ToList();
            ViewBag.data = par;

            using (AceDBEntities entity = new AceDBEntities())
            { 
                return View(entity.Payments.Where(x => x.confirmation_id == id).FirstOrDefault());
            }

        }

        [HttpPost]
        public ActionResult Approval(int id, Payment payment)
        {

            try
            {
                // TODO: Add update logic here
                using (AceDBEntities entity = new AceDBEntities())
                {
                    entity.Entry(payment).State = EntityState.Modified;
                    entity.SaveChanges();
                }
                return RedirectToAction("ApprovalList","Payment");
            }
            catch
            {
                return View();
            }

        }

        public ActionResult PaymentHistories()
        {
            //AceDBEntities entity = new AceDBEntities();
            //List<Payment> payment = entity.Payments.ToList();
            //List<Status> status = entity.Status.ToList();

            //var multipletable = from a in payment
            //                    join b in status on a.status_id equals b.status_id into table1
            //                    from b in table1.DefaultIfEmpty()
            //                    select new MultipleClass { paymentdetails = a, statusdetails = b };

            //return View(multipletable);

            AceDBEntities entity = new AceDBEntities();
            List<Payment> payment = entity.Payments.ToList();
            List<Status> status = entity.Status.ToList();
            List<Parent> parent = entity.Parents.ToList();
            List<Student> student = entity.Students.ToList();
            List<Package> package = entity.Packages.ToList();

            var multipletable = from a in payment
                                join b in status on a.status_id equals b.status_id
                                join c in parent on a.parent_ic equals c.parents_ic
                                join d in student on a.student_ic equals d.student_ic
                                join e in package on a.package_id equals e.package_id

                                select new MultipleClass { paymentdetails = a, statusdetails = b, parentdetails = c, studentdetails = d, packagedetails = e };

            return View(multipletable);
        }

        // POST: Payment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Payment payment = db.Payments.Find(id);
            db.Payments.Remove(payment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
