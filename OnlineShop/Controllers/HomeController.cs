using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.Dao;
using OnlineShop.Models;
using OnlineShop.Common;
using Model.EF;
using OnlineShop.Areas.Admin.Controllers;

namespace OnlineShop.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            var product = new ProductDao();
            ViewBag.AllProduct = product.ListProduct();
            ViewBag.NewProduct = product.ListNewProduct(4);
            ViewBag.NewProduct2 = product.ListNewProduct2(4);
            return View();
        }
        [ChildActionOnly]
        public PartialViewResult HeaderCart()
        {
            var cart = Session[CommonConstant.CartSession];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return PartialView(list);
        }
        
    }
}