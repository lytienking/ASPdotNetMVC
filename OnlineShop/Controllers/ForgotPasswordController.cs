using Common;
using Model.Dao;
using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Controllers
{
    public class ForgotPasswordController : Controller
    {
        // GET: ForgotPassword
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(ForgotPassModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                if (dao.CheckUsername(model.Username) && dao.CheckEmail(model.Gmail))
                {
                    var user = dao.GetUserName(model.Username);
                    if(user.Username==model.Username && user.Email==model.Gmail)
                    {
                        string code = CodeChangePass(6);
                        user.Code = code;
                        dao.Update(user);
                        string content = System.IO.File.ReadAllText(Server.MapPath("/assets/net/template/newCode.html"));
                        content = content.Replace("{{Code}}", code);
                        new MailHelper().SendEmail(model.Gmail, "code to change the password", content);
                        return Redirect("/ResetPassword/Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Email hoặc Username không đúng");
                    }
                    
                }
                else
                {
                    ModelState.AddModelError("", "Email hoặc Username không đúng");
                }
            }
            return View(model);
        }
        private static string CodeChangePass(int lenght)
        {
            string LetterCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            string NumberCharacters = "0123456789";
            char[] code = new char[lenght];
            Random rd = new Random();
            bool useCharacters = true;
            for (int i = 0; i < lenght; i++)
            {
                if (useCharacters)
                {
                    code[i] = LetterCharacters[rd.Next(0, LetterCharacters.Length)];
                    useCharacters = false;
                }
                else
                {
                    code[i] = NumberCharacters[rd.Next(0, NumberCharacters.Length)];
                    useCharacters = true;
                }
            }
            return new string(code);
        }
    }
}