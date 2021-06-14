using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using AceTC.Models;

namespace AceTC.Controllers
{
    public class OutstandingController : Controller
    {
        // GET: Oustanding/Index
        public ActionResult Index()
        {


            AceDBEntities entity = new AceDBEntities();
            List<Outstanding> outs = entity.Outstandings.ToList();
            List<Status> stats = entity.Status.ToList();

            var stattable = from o in outs
                            join s in stats on o.O_status equals s.status_id
                            select new MultipleClass { statusdetails = s, outstandingdetails = o };
            return View(stattable);
        }

        public ActionResult ApprovalOutstanding(int id)
        {
            AceDBEntities entity = new AceDBEntities();
            Outstanding outs = entity.Outstandings.Find(id);
            List<SelectListItem> b = new List<SelectListItem>()
            {
                new SelectListItem {
                    Text = "Approved", Value = "2"
                },
                new SelectListItem {
                    Text = "Pending", Value = "1"
                },
            };
            ViewBag.O_status = new SelectList(b, "Value", "Text");
            if (outs == null)
                return View("NotFound");
            else
                return View(outs);
        }
        [HttpPost]
        public ActionResult ApprovalOutstanding(Outstanding outstanding)
        {
            using (AceDBEntities entity = new AceDBEntities())
            {
                entity.Entry(outstanding).State = EntityState.Modified;
                entity.SaveChanges();
            }
            ModelState.Clear();
            //ViewBag.SuccessMessage = "Save Changes Successful. ";
            return RedirectToAction("Index", "Outstanding");
        }


        public ActionResult SendOutstandingEmail()
        {
            AceDBEntities db = new AceDBEntities();
            ViewBag.ParentList = new SelectList(GetParentList(), "parents_ic", "parents_email");
            return View();
        }
        public List<Parent> GetParentList()
        {
            AceDBEntities db = new AceDBEntities();
            List<Parent> parents = db.Parents.ToList();
            return parents;
        }

        public ActionResult GetOutstandingList(string receiver)
        {
            AceDBEntities db = new AceDBEntities();
            List<Outstanding> selectList = db.Outstandings.Where(x => x.O_pID == receiver).ToList();
            ViewBag.Slist = new SelectList(selectList, "O_ID", "O_fees");
            return PartialView("DisplayOutstanding");
        }

        [HttpPost]
        public ActionResult SendOutstandingEmail(string receiver, string subject, string message)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var senderEmail = new MailAddress("nurulasyikin.s390@gmail.com", "Ace Education");
                    var receiverEmail = new MailAddress(receiver, "Receiver");
                    var password = "Nurul990626_";
                    var sub = subject;
                    var body = message;
                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(senderEmail.Address, password)
                    };
                    using (var mess = new MailMessage(senderEmail, receiverEmail)
                    {
                        Subject = subject,
                        Body = "Total Outstanding Balance " + body
                    })
                    {
                        smtp.Send(mess);
                    }

                    ViewBag.msg = "Email sent successfully";

                    return View();
                }
            }
            catch (Exception)
            {
                ViewBag.Error = "Send Email failed!!";
            }
            return View();
        }
   

    public ActionResult ApprovalAll ()
        {
            AceDBEntities db = new AceDBEntities();
            List<Student> studentlist = db.Students.ToList();
            List<Parent> parent = db.Parents.ToList();
            List<Package> package = db.Packages.ToList();
            List<Payment> paymentlist = db.Payments.ToList();

            var multiple = from o in studentlist
                           join s in package on o.student_package equals s.package_id
                           join p in parent on o.parent_ic equals p.parents_ic
                           select new MultipleClass { packagedetails = s, studentdetails = o , parentdetails = p};

            var generate = from s in studentlist
                           join p in package on s.student_package equals p.package_id
                           join py in paymentlist on s.student_ic equals py.student_ic
                           select new MultipleClass { packagedetails = p, studentdetails = s, paymentdetails = py };



            DateTime nextMonth = DateTime.Now.AddMonths(1);
            DateTime todayDate = DateTime.Now;
            DateTime payDate = new DateTime(todayDate.Year, todayDate.Month, 14);
            int result = DateTime.Compare(todayDate, payDate);


            foreach (var oldpay in generate)
            {

                Outstanding newpay = new Outstanding();
                newpay.O_month = DateTime.Now;
                newpay.O_pID = oldpay.studentdetails.parent_ic;
                newpay.O_fees = oldpay.paymentdetails.payment_fee;
                newpay.O_remark = "test ";
                newpay.O_month = nextMonth;
                newpay.O_status = 1;


                db.Outstandings.Add(newpay);
                db.SaveChanges();


            }
          /*  return RedirectToAction("Index", "Outstanding");*/

            return View(multiple);
        }
    
/*    [HttpPost]
    public ActionResult ApprovalAll()
        {
            AceDBEntities db = new AceDBEntities();
            List<Student> studentlist = db.Students.ToList();
            List<Parent> parent = db.Parents.ToList();
            List<Package> package = db.Packages.ToList();
            List<Payment> paymentlist = db.Payments.ToList();

             var generate = from s in studentlist
                            join p in package on s.student_package equals p.package_id
                            join py in paymentlist on s.student_ic equals py.student_ic
                            select new MultipleClass { packagedetails = p, studentdetails = s, paymentdetails = py };

            
            
            DateTime nextMonth = DateTime.Now.AddMonths(1);
            DateTime todayDate = DateTime.Now;
            DateTime payDate = new DateTime(todayDate.Year, todayDate.Month, 14);
            int result = DateTime.Compare(todayDate, payDate);


            foreach (var oldpay in generate)
            {
                
                Outstanding newpay = new Outstanding();
                newpay.O_month = DateTime.Now;
                newpay.O_pID = oldpay.parentdetails.parents_ic;
                newpay.O_fees = oldpay.paymentdetails.payment_fee;
                newpay.O_remark = "test ";
                newpay.O_month = nextMonth;
                newpay.O_status = 1;


                db.Outstandings.Add(newpay);
                db.SaveChanges();


            }
            return RedirectToAction("Index", "Outstanding");

            return View();
        }*/






    }
}
