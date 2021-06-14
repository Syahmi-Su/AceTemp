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
            //using (AceDBEntities entity = new AceDBEntities())
            //{
            //    return View(entity.Outstandings.ToList());
            //}

            //AceDBEntities entity = new AceDBEntities();
            //List<Student> studentparent = entity.Students.ToList();
            //List<Package> packagename = entity.Packages.ToList();

            //var multable = from pc in packagename
            //               join s in studentparent on pc.package_id equals s.student_package
            //               select new MultipleClass { studentdetails = s, packagedetails = pc };
            //return View(multable);


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

        //// GET: Oustanding/Details/5
        //public ActionResult Details(int id)
        //{
        //    using (AceDBEntities entity = new AceDBEntities())
        //    {
        //        return View(entity.Outstandings.Where(x => x.O_ID == id).FirstOrDefault());
        //    }
        //}

        //// GET: Oustanding/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Oustanding/Create
        //[HttpPost]
        //public ActionResult Create(Outstanding outstanding)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here
        //        using (AceDBEntities entity = new AceDBEntities())
        //        {
        //            entity.Outstandings.Add(outstanding);
        //            entity.SaveChanges();
        //        }
        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: Oustanding/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    using (AceDBEntities entity = new AceDBEntities())
        //    {
        //        return View(entity.Outstandings.Where(x => x.O_ID == id).FirstOrDefault());
        //    }
        //}

        //// POST: Oustanding/Edit/5
        //[HttpPost]
        //public ActionResult Edit(int id, Outstanding outstanding)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here
        //        using (AceDBEntities entity = new AceDBEntities())
        //        {
        //            entity.Entry(outstanding).State = EntityState.Modified;
        //            entity.SaveChanges();
        //        }
        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: Oustanding/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    using (AceDBEntities entity = new AceDBEntities())
        //    {
        //        return View(entity.Outstandings.Where(x => x.O_ID == id).FirstOrDefault());
        //    }
        //}

        //// POST: Oustanding/Delete/5
        //[HttpPost]
        //public ActionResult Delete(int id, Outstanding outstanding)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here
        //        using (AceDBEntities entity = new AceDBEntities())
        //        {
        //            outstanding = entity.Outstandings.Where(x => x.O_ID == id).FirstOrDefault();
        //            entity.Outstandings.Remove(outstanding);
        //            entity.SaveChanges();
        //        }
        //            return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

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
                        Body = "Total Outstanding Balance" + body
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
    }
}
