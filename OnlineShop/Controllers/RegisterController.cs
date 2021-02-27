using Model.Dao;
using Model.EF;
using OnlineShop.Common;
using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Controllers
{
    public class RegisterController : Controller
    {
        // GET: Register
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                if(dao.CheckUsername(model.Username))
                {
                    ModelState.AddModelError("", "Tên đăng nhập đã tồn tại");
                }
                else if(dao.CheckEmail(model.Email))
                {
                    ModelState.AddModelError("", "Tên Email đã tồn tại");
                }
                else
                {
                    var user = new User();
                    user.Name = model.Name;
                    user.Phone = model.Phone;
                    user.Address = model.Address;
                    user.Email = model.Email;
                    user.Username = model.Username;
                    user.Password =Encrytor.MD5Hash( model.Password);
                    user.CreatedDate = DateTime.Now;
                    user.Status = true;
                    user.GroupID ="MEMBER";
                    var result = dao.InSert(user);
                    if(result>0)
                    {
                        ViewBag.Success = "Đăng kí thành công";
                        model = new RegisterModel();
                    }
                    else
                    {
                        ModelState.AddModelError("", "Đăng kí không thành công");
                    }
                }
            }
            return View(model);
        }
    }
}