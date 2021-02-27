using Model.Dao;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using OnlineShop.Middleware;
using Newtonsoft.Json;
using Model.ViewModel;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class ChartController : Controller
    {
        // GET: Admin/Statical
        OnlineShopDbContext db = new OnlineShopDbContext();
        public ActionResult Index()
        {


            List<int> a = new List<int>();
            IQueryable<OrderDetail> model = db.OrderDetails;
            var Id = model.Select(x => x.ProductID).Distinct();
            foreach (var item in Id)
            {

                a.Add(model.Count(x => x.ProductID == item));
            }
            var b = a;
            ViewBag.ID = Id;
            ViewBag.Quantity = b.ToList();



            IQueryable<Product> model2 = db.Products;
            var Id2 = model2.Select(x => x.ID);
            var tonkho = model2.Select(x => x.Quantity);
            ViewBag.MaSP = Id2;
            ViewBag.SoLuong = tonkho.ToList();

            List<int> sl3 = new List<int>();
            IQueryable<Order> model3 = db.Orders;
            var Id3 = model3.Select(x => x.CustomerID).Distinct();
            foreach (var item in Id3)
            {
                sl3.Add(model3.Count(x => x.CustomerID == item));
            }
            ViewBag.MaKH = Id3;
            ViewBag.SoLuong3 = sl3.ToList();

            List<MaxOrder> model4 = GetListMaxOrder();

            var id4 = model4.Select(x => x.name).Distinct();
            var sl4 = model4.Select(x => x.quantity);
            ViewBag.id4 = id4;
            ViewBag.sl4 = sl4;


            List<BestCustomer> model5 = GetListBestCustomer();
            var id5 = model5.Select(x => x.name).Distinct();
            var sl5 = model5.Select(x => x.quantity);
            ViewBag.id5 = id5;
            ViewBag.sl5 = sl5;
            return View();

        }
        public List<MaxOrder> GetListMaxOrder()
        {
            List<OrderDetail> lines = db.OrderDetails.ToList();
            List<MaxOrder> result = lines.GroupBy(i => i.ProductID).SelectMany(cl => cl.Select(
                  csLine => new MaxOrder
                  {
                      name = csLine.ProductID,
                      quantity = cl.Sum(c => c.Quantity)
                  })).ToList<MaxOrder>();
            List<MaxOrder> rs2 = new List<MaxOrder>();
            foreach (var p in result)
            {
                if (p.quantity > 2)
                {
                    rs2.Add(p);
                }
            }
            return rs2;
        }
        public List<BestCustomer> GetListBestCustomer()
        {
            List<Order> lines = db.Orders.ToList();
            List<BestCustomer> result = lines.GroupBy(i => i.CustomerID).SelectMany(cl => cl.Select(
                  csLine => new BestCustomer
                  {
                      name = csLine.CustomerID,
                      quantity = cl.Count().ToString()

                  })).ToList<BestCustomer>();
            List<BestCustomer> rs2 = new List<BestCustomer>();
            foreach (var p in result)
            {
                int a = Convert.ToInt32(p.quantity);
                if (a  >=1)
                {
                    rs2.Add(p);
                }
            }
            return rs2;
        }


    }
}
