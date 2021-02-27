using Model.Dao;
using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Controllers
{
    public class ComfirmOrderController : Controller
    {
        // GET: ComfirmOrder
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(CodeOrderModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new OrderDao();

                var user = dao.GetCode(model.Code);
                if (user.Code == model.Code)
                {
                    user.Status = 2;
                    dao.Update(user);
                    ModelState.AddModelError("", "Xác nhận thành công");
                    user.Code = null;
                    dao.Update(user);
                }
                else
                {
                    ModelState.AddModelError("", "Bạn đã nhập sai code!");
                }

            }
            return View("Index");
        }
    }
}