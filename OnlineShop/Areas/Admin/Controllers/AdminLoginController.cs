using Model.Dao;
using OnlineShop.Areas.Admin.Models;
using OnlineShop.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class AdminLoginController : Controller
    {
        // GET: Admin/AdminLogin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Logout()
        {
            Session[CommonConstant.USER_SESSION] = null;
            return Redirect("/Admin/AdminLogin/Index");
        }
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                var result = dao.Login(model.Username, Encrytor.MD5Hash(model.Password) , true);
                if (result == 1)
                {
                    var user = dao.GetUserName(model.Username);
                    var userSesstion = new UserLogin();
                    userSesstion.Username = user.Username;
                    userSesstion.UserID = user.ID;
                    userSesstion.GroupID = user.GroupID;               
                    var listCredential = dao.GetlistCredential(model.Username);
                    Session.Add(CommonConstant.SESSION_CREDENTIALS, listCredential);
                    Session.Add(CommonConstant.USER_SESSION,userSesstion);
                    Session["USER_SESSION"] = user;
                    return Redirect("/Admin/HomeAdmin/Index");
                }
                else
                {
                    if (result == 0)
                    {
                        ModelState.AddModelError("", "Tài khoản không tồn tại");
                    }
                    else
                        if (result == -1)
                    {
                        ModelState.AddModelError("", "Đăng nhập không đúng");
                    }
                    else
                        if (result == -2)
                    {
                        ModelState.AddModelError("", "Tài khoản đang bị khóa");
                    }
                    else
                        if (result == -3)
                    {
                        ModelState.AddModelError("", "Bạn không có quyền đăng nhập vào");
                    }
                }

            }
            return View("Index");
        }
    }
}