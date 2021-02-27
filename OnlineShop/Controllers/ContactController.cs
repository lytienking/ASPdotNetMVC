using Model.Dao;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Send(string content)
        {
            var session = (UserLogin)Session[OnlineShop.Common.CommonConstant.USER_SESSION];
            var contact = new Contact();
            if (session != null)
            {
                contact.Content = content;
                contact.CustomerID = session.UserID;
                contact.Status = false;
            }
           
            var result = new ContactDao().Insert(contact);
            if (result > 0)
            {
                return Json(new
                {
                    status = true
                });
            }
            else
            {
                return Json(new
                {
                    status = false
                });

            }
        }
    }
}