using Model.Dao;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {
            var productDao = new ProductDao();
            ViewBag.Product = productDao.ListProduct();
            return View();
        }
        public ActionResult Detail(int Id)
        {
            var product = new ProductDao().ViewDetail(Id);
            var commentDao = new CommentDao();
            ViewBag.listfeedback = commentDao.ListFeedBack();
            return View(product);
        }
        [HttpPost]
        public JsonResult Comment(string content,int id)
        {
            var product = new ProductDao().ViewDetail(id);
            var session = (UserLogin)Session[OnlineShop.Common.CommonConstant.USER_SESSION];
            var feedback = new FeedBack();
            if (session != null)
            {
                feedback.ProductID = product.ID;
                feedback.Content = content;
                feedback.Customer = session.Username;
                feedback.CreatedDate = DateTime.Now;
                feedback.Status = false;
            }
            var result = new CommentDao().Insert(feedback);
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
        public JsonResult ListName(string term)
        {
            var data = new ProductDao().ListName(term);
            return Json(new
            {
                data = data,
                status = true
            },JsonRequestBehavior.AllowGet);
        }       
       public ActionResult Category(string cateID,int page=1,int pageSize=4)
        {
            if (string.IsNullOrEmpty(cateID))
            {
                return Redirect("/Home/Index");
            }
            var id = int.Parse(cateID);
            var category = new ProductCategoryDao();
            ViewBag.Category = category.ViewDetail(id);
            var productDao = new ProductDao();
            int totalRecord = 0;
            ViewBag.Product = productDao.ListByCategoryID(id,ref totalRecord, page, pageSize);
            ViewBag.Total = totalRecord;
            ViewBag.Page = page;
            int maxPage = 5;
            int totalPage = 0;
            totalPage =(int)Math.Ceiling((decimal)(totalRecord / pageSize))+1;
            ViewBag.TotalPage = totalPage;
            ViewBag.MaxPage = maxPage;
            ViewBag.First = 1;
            ViewBag.Last = totalPage;
            ViewBag.Next = page + 1;
            ViewBag.Prev = page - 1;
            return View();
        }
        public ActionResult Search(string keyword, int page = 1, int pageSize = 4)
        {
            var productDao = new ProductDao();
            int totalRecord = 0;
            ViewBag.Product = productDao.Search(keyword, ref totalRecord, page, pageSize);
            ViewBag.Keyword = keyword;
            ViewBag.Total = totalRecord;
            ViewBag.Page = page;
            int maxPage = 10;
            int totalPage = 0;
            totalPage = (int)Math.Ceiling((decimal)(totalRecord / pageSize)) + 1;
            ViewBag.TotalPage = totalPage;
            ViewBag.MaxPage = maxPage;
            ViewBag.First = 1;
            ViewBag.Last = totalPage;
            ViewBag.Next = page + 1;
            ViewBag.Prev = page - 1;
            return View();
        }
        [ChildActionOnly]
        public PartialViewResult ProductCategory()
        {
            var model = new ProductCategoryDao().ListAll();
            return PartialView(model);
        }
        [ChildActionOnly]
        public PartialViewResult ProductCategory2()
        {
            var model = new ProductCategoryDao().ListAll();
            return PartialView(model);
        }
    }
}