using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using AceTC.Models;

namespace AceTC.Controllers
{
    public class EmailController : Controller
    {
        public ActionResult SendEmail()
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
        public ActionResult SendEmail(string receiver, string subject, string message)
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
                ViewBag.Error = "Some Error";
            }
            return View();
        }

    }
}