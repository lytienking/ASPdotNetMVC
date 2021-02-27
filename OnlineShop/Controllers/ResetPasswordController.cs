using Model.Dao;
using OnlineShop.Common;
using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Controllers
{
    public class ResetPasswordController : Controller
    {
        // GET: ResetPassword
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                if (dao.CheckUsername(model.Username))
                {
                    var user = dao.GetUserName(model.Username);
                    if (user.Code == model.Code)
                    {
                        user.Password = Encrytor.MD5Hash(model.Password);
                        dao.Update(user);
                        ModelState.AddModelError("", "Đổi mật khẩu thành công");
                        user.Code = null;
                        dao.Update(user);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Bạn đã nhập sai code!");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại");
                }
            }
            return View(model);
        }
    }
}