using Model.Dao;
using Model.EF;
using OnlineShop.Areas.Admin.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Controllers
{
    public class EditInfoController : BaseController
    {
        // GET: EditInfo
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Info(string id)
        {

            if (string.IsNullOrEmpty(id))
            {
                return Redirect("/");
            }
            var idPa = int.Parse(id);
            var user = new UserDao().ViewDetail(idPa);
            var dao = new ProductDao();
            ViewBag.ListBuy = dao.ListBuy();
            return View(user);
        }
        [HttpPost]
        public ActionResult Info(User user)
        {

            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                var result = dao.Update(user);

                if (result)
                {
                    ModelState.AddModelError("", "Sửa thông tin thành thành công");
                    return View("Info");
                }
                else
                {
                    ModelState.AddModelError("", "Sửa thông tin không thành thành công");
                    return View("Info");
                }
            }

            return View("Info");
        }
    }
}