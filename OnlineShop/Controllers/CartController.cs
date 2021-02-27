using Common;
using Model.Dao;
using Model.EF;
using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace OnlineShop.Controllers
{
    public class CartController : Controller
    {
        private const string CartSession = "CartSession";
        // GET: Cart
        public ActionResult Index()
        {
            var cart = Session[CartSession];
            var list=new List<CartItem>();
            if(cart!=null)
            {
                list = (List<CartItem>)cart;
            }
            return View(list);
        }
        public ActionResult AddItem(long productId,int quantity,string size)
        {
            var product = new ProductDao().ViewDetail(productId);
            var cart = Session[CartSession];
            if(cart!=null)
            {
                var list = (List<CartItem>)cart;
                if(list.Exists(x=>x.Product.ID==productId))
                {
                    foreach (var item in list)
                    {
                        if (item.Product.ID == productId)
                        {
                            item.Quantity += quantity;
                        }
                    }
                }else
                {
                    var item = new CartItem();
                    item.Product = product;
                    item.Quantity = quantity;
                    item.Size = size;
                    list.Add(item);
                    
                }
                Session[CartSession] = cart;
            }
            else
            {
                var item = new CartItem();
                item.Product = product;
                item.Quantity = quantity;
                item.Size = size;
                var list = new List<CartItem>();
                list.Add(item);
                Session[CartSession] = list;
            }
            return RedirectToAction("Index");
        }
        public JsonResult Update(string cartModel)
        {
            var jsonCart = new JavaScriptSerializer().Deserialize<List<CartItem>>(cartModel);
            var sessionCart = (List<CartItem>)Session[CartSession];
            foreach(var item in sessionCart)
            {
                var jsonItem = jsonCart.SingleOrDefault(x => x.Product.ID == item.Product.ID);
                if(jsonItem!=null)
                {
                    item.Size = jsonItem.Size;
                    item.Quantity = jsonItem.Quantity;
                }
            }
            Session[CartSession] = sessionCart;
            return Json(new
            {
                status = true
            });
        }
        public JsonResult DeleteAll()
        {
            Session[CartSession] = null;
            return Json(new
            {
                status = true
            });
        }
        public JsonResult Delete(long id)
        {
            var sessionCart = (List<CartItem>)Session[CartSession];
            sessionCart.RemoveAll(x => x.Product.ID == id);
            Session[CartSession] = sessionCart;
            return Json(new
            {
                status = true
            });
        }
        public ActionResult Payment()
        {
            var cart = Session[CartSession];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return View(list);
        }
        [HttpPost]
        public ActionResult Payment(string name, string address, string phone, string email)
        {
            var session = (UserLogin)Session[OnlineShop.Common.CommonConstant.USER_SESSION];
            var order = new Order();
            string code = CodeChangePass(6);
            if (session!=null)
            {
                order.CreatedDate = DateTime.Now;
                order.ShipAddress = address;
                order.ShipEmail = email;
                order.ShipMobile = phone;
                order.ShipName = name;
                order.CustomerID = session.UserID;
                order.Status = 1;
                order.Code = code;
            }           
            try
            {
                var id = new OrderDao().Insert(order);
                var cart = (List<CartItem>)Session[CartSession];
                var detailDao = new OrderDetailDao();
                decimal total = 0;
                foreach (var item in cart)
                {
                    var orderdetail = new OrderDetail();
                    orderdetail.ProductID = item.Product.ID;
                    orderdetail.OrderID = id;
                    orderdetail.Size = item.Size;
                    orderdetail.Quantity = item.Quantity;
                    orderdetail.Price = item.Product.Price;
                    detailDao.Insert(orderdetail);
                    total += (item.Product.Price.GetValueOrDefault(0) * item.Quantity);
                }
                string content = System.IO.File.ReadAllText(Server.MapPath("/assets/net/template/neworder.html"));
                content = content.Replace("{{CustomerName}}", name);
                content = content.Replace("{{Phone}}", phone);
                content = content.Replace("{{Email}}", email);
                content = content.Replace("{{Address}}", address);
                content = content.Replace("{{Code}}", code);
                content = content.Replace("{{Total}}", total.ToString());
                new MailHelper().SendEmail(email, "Xác nhận đơn hàng", content);
            }
            catch (Exception)
            {
                throw;
            }
            Session[CartSession] = null;
            return Redirect("/hoan-thanh");
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
        public ActionResult Success()
        {
            return View();
        }
    }
}