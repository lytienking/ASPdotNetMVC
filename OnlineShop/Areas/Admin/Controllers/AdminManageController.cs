using Model.Dao;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using OnlineShop.Middleware;
using OnlineShop.Common;
using System.Data;
using System.IO;
using ClosedXML;
using ClosedXML.Excel;

namespace OnlineShop.Areas.Admin.Controllers
{
    [VerifyAdmin]
    public class AdminManageController : BaseController
    {
        // GET: Admin/AdminManage
        [HasCredential(RoleID ="VIEW_USER")]
        public ActionResult Index(string searchString, int page = 1, int pageSize = 5)
        {
            var dao = new UserDao();
            var model = dao.ListAllPaging(searchString, page, pageSize);
            return View(model);
        }
        public ActionResult ContactIndex(string searchString, int page = 1, int pageSize = 5)
        {
            var dao = new ContactDao();
            var model = dao.ListAllPaging(searchString, page, pageSize);
            return View(model);
        }
        [HasCredential(RoleID = "VIEW_MOD")]
        public ActionResult ListModIndex(string searchString, int page = 1, int pageSize = 5)
        {
            var dao = new CredentialDao();
            var model = dao.ListMod(searchString, page, pageSize);
            return View(model);
        }
        [HasCredential(RoleID = "VIEW_PRODUCT")]
        public ActionResult ProductIndex(string searchString, int page = 1, int pageSize = 5)
        {
            var dao = new ProductDao();
            var model = dao.ListAllPaging(searchString, page, pageSize);
            return View(model);
        }
        [HasCredential(RoleID = "VIEW_FEEDBACK")]
        public ActionResult FeedBackIndex(string searchString, int page = 1, int pageSize = 5)
        {
            var dao = new CommentDao();
            var model = dao.ListAllPaging(searchString, page, pageSize);
            return View(model);
        }
        [HasCredential(RoleID = "VIEW_ORDER")]
        public ActionResult OrderIndex(string searchString, int page = 1, int pageSize = 5)
        {
            var dao = new OrderDao();
            string status = Request.QueryString["status"];
            IEnumerable<Order> data;
            if (status != null && status != "")
            {
                data = dao.viaStatus(int.Parse(status)).ListAllPaging(searchString, page, pageSize);
            }
            else
            {
                data = dao.ListAllPaging(searchString, page, pageSize);
            }
           // var model = dao.ListAllPaging(searchString, page, pageSize);
            return View(data);
        }
        [HasCredential(RoleID ="VIEW_PRODUCTCATEGORY")]
        public ActionResult ProductCategoryIndex(string searchString, int page = 1, int pageSize = 5)
        {
            var dao = new ProductCategoryDao();
            var model = dao.ListAllPaging(searchString, page, pageSize);
            return View(model);
        }
        [HasCredential(RoleID = "VIEW_PARENTIDCATEGORY")]
        public ActionResult ParentIDCategoryIndex(string searchString, int page = 1, int pageSize = 5)
        {
            var dao = new ParentIDCategoryDao();
            var model = dao.ListAllPaging(searchString, page, pageSize);
            return View(model);
        }
        [HasCredential(RoleID = "VIEW_ORDERDETAIL")]
        public ActionResult OrderDetail(long id)
        {
            var dao = new OrderDetailDao();
            var model = dao.OrderDetail(id);
            return View(model);
        }
        [HttpGet]
        [HasCredential(RoleID = "ADD_PRODUCT")]
        public ActionResult CreateProduct()
        {
            SetViewBag();
            return View();
        }
        [HttpPost]
        [HasCredential(RoleID = "ADD_PRODUCT")]
        public ActionResult CreateProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                var dao = new ProductDao();
                product.CreatedDate = DateTime.Now;
                long id = dao.InSert(product);
                if (id > 0)
                {
                    SetAlert("Thêm sản phẩm thành công", "success");
                    return Redirect("/Admin/AdminManage/ProductIndex");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm sản phẩm không thành thành công");
                }
            }
            SetViewBag();
            return View("ProductIndex");
        }
        public void SetViewBag(long? selectedId = null)
        {
            var dao = new ProductCategoryDao();
            ViewBag.CategoryID = new SelectList(dao.ListAll(), "ID", "Name", selectedId);
        }
        [HttpGet]
        [HasCredential(RoleID = "ADD_PRODUCTCATEGORY")]
        public ActionResult CreateProductCategory()
        {
            SetViewBagParentID();
            return View();
        }
        [HttpPost]
        [HasCredential(RoleID = "ADD_PRODUCTCATEGORY")]
        public ActionResult CreateProductCategory(ProductCategory productCategory)
        {
            if (ModelState.IsValid)
            {
                var dao = new ProductCategoryDao();
                productCategory.CreatedDate  = DateTime.Now;
                long id = dao.InSert(productCategory);
                if (id > 0)
                {
                    SetAlert("Thêm danh mục sản phẩm thành công", "success");
                    return Redirect("/Admin/AdminManage/ProductCategoryIndex");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm danh mục sản phẩm không thành thành công");
                }
            }
            SetViewBagParentID();
            return View("ProductCategoryIndex");
        }
        public void SetViewBagParentID(long? selectedId = null)
        {
            var dao = new ParentIDCategoryDao();
            ViewBag.ParentID = new SelectList(dao.ListAll(), "ID", "Name", selectedId);
        }
        public void SetViewBagMod(long? selectedId = null)
        {
            var dao = new CredentialDao();
            ViewBag.RoleID = new SelectList(dao.ListAll(), "Name", "Name", selectedId);
        }
        [HttpGet]
        [HasCredential(RoleID = "ADD_USER")]
        public ActionResult Create()
        {
            return View();
        }
        [HttpGet]
        [HasCredential(RoleID = "ADD_MOD")]
        public ActionResult CreateMod()
        {
            SetViewBagMod();
            return View();
        }
        [HttpPost]
        [HasCredential(RoleID = "ADD_MOD")]
        public ActionResult CreateMod(Credential Cre)
        {
            if (ModelState.IsValid)
            {
                var dao = new CredentialDao();
                long id = dao.InSert(Cre);
                if (id > 0)
                {
                    SetAlert("Thêm Mod thành công", "success");
                    return Redirect("/Admin/AdminManage/ListModIndex");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm Mod không thành thành công");
                }
            }
            SetViewBagMod();
            return View("ListModIndex");
        }
        [HttpPost]
        [HasCredential(RoleID = "ADD_USER")]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                user.CreatedDate = DateTime.Now;                   
                if (dao.CheckUsername(user.Username))
                {
                    SetAlert("Tên đăng nhập bị trùng", "warning");
                    return View("Create");
                }
                else
                {
                    if(dao.CheckEmail(user.Email))
                    {
                        SetAlert("Tên email bị trùng", "warning");
                        return View("Create");
                    }
                    else
                    {
                        long id = dao.InSert(user);
                        if (id > 0)
                        {
                            SetAlert("Thêm user thành công", "success");
                            return Redirect("/Admin/AdminManage/Index");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Thêm user không thành công");
                        }
                    }
                }
            }

            return View("Index");    
        }
        [HasCredential(RoleID = "EDIT_USER")]
        public ActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Redirect("/Admin/AdminManage/Index");
            }
            var idPa = int.Parse(id);
            var dao = new UserDao();
            var user = dao.ViewDetail(idPa);
            return View(user);
        }        
        [HttpPost]
        [HasCredential(RoleID = "EDIT_USER")]
        public ActionResult Edit(User user)
        {

            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                var result = dao.Update(user);

                if (result)
                {
                    SetAlert("Sửa User thành công", "success");
                    return Redirect("/Admin/AdminManage/Index");
                }
                else
                {
                    ModelState.AddModelError("", "Sửa user không thành thành công");
                }
            }

            return View("Index");
        }
        [HasCredential(RoleID = "EDIT_PRODUCT")]
        public ActionResult EditProduct(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Redirect("/Admin/AdminManage/ProductIndex");
            }
            var idPa = int.Parse(id);
            var product = new ProductDao().ViewDetail(idPa);
            SetViewBag();
            return View(product);
        }
        [HttpPost]
        [HasCredential(RoleID = "EDIT_PRODUCT")]
        public ActionResult EditProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                var dao = new ProductDao();
                var result = dao.Update(product);

                if (result)
                {
                    SetAlert("Sửa sản phẩm thành công", "success");
                    return Redirect("/Admin/AdminManage/ProductIndex");
                }
                else
                {
                    ModelState.AddModelError("", "Sửa sản phẩm không thành thành công");
                }
            }
            SetViewBag();
            return View("ProductIndex");
        }
        [HttpDelete]
        [HasCredential(RoleID = "DELETE_USER")]
        public ActionResult Delete(int id)
        {
            new UserDao().Delete(id);
            return Redirect("/Admin/AdminManage/Index");
        }
        [HttpDelete]
        [HasCredential(RoleID = "DELETE_PRODUCT")]
        public ActionResult DeleteProduct(int id)
        {
            new ProductDao().Delete(id);
            return Redirect("/Admin/AdminManage/ProductIndex");
        }
        [HttpDelete]
        [HasCredential(RoleID = "DELETE_MOD")]
        public ActionResult DeleteMod(int id)
        {
            new CredentialDao().Delete(id);
            return Redirect("/Admin/AdminManage/ListModIndex");
        }
        [HttpDelete]
        [HasCredential(RoleID = "DELETE_FEEDBACK")]
        public ActionResult DeleteFeedBack(int id)
        {
            new CommentDao().Delete(id);
            return Redirect("/Admin/AdminManage/FeedBackIndex");
        }
        [HttpDelete]
        [HasCredential(RoleID = "DELETE_CONTACT")]
        public ActionResult DeleteContact(int id)
        {
            new ContactDao().Delete(id);
            return Redirect("/Admin/AdminManage/ContactIndex");
        }
        [HttpDelete]
        [HasCredential(RoleID = "DELETE_ORDER")]
        public ActionResult DeleteOrder(int id)
        {

            new OrderDao().Delete(id);
            return Redirect("/Admin/AdminManage/OrderIndex");
        }
        [HttpDelete]
        [HasCredential(RoleID = "DELETE_DETAIL")]
        public ActionResult DeleteDetail(int id,int orderid)
        {
            
            var dao = new OrderDetailDao();
            var result = dao.Delete(id, orderid);
            return Redirect("/Admin/AdminManage/OrderDetail");
        }
        [HttpPost]
        public JsonResult ChangeStatus(long id)
        {
            var dao = new UserDao();
            var result = dao.ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }
        [HttpPost]
        public JsonResult ChangeStatusContact(long id)
        {
            var dao = new UserDao();
            var result = dao.ChangeStatusContact(id);
            return Json(new
            {
                status = result
            });
        }
        [HttpPost]
        public JsonResult ChangeStatusOrder(int bill_id, int status)
        {
            OrderDao dao = new OrderDao();
            dao.changeStatus(bill_id, status);
            SetAlert("Sửa trạng thái thành công", "success");
            return Json(new { status = true }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public FileResult Export()
        {

            OnlineShopDbContext db = new OnlineShopDbContext();
            List<int> l = new List<int>();
            IQueryable<Product> model = db.Products;
            var Id = model.Select(x => x.ID);
            foreach (var item in Id)
            {
                l.Add(model.Count(x => x.ID == item));
            }
            int a = l.Count();
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[4]
                {
                    new DataColumn ("Id"),
                    new DataColumn("Name"),
                    new DataColumn("Price"),
                    new DataColumn("Quantity")
                });
            var products = from Product in db.Products.Take(a) select Product;
            foreach (var p in products)
            {
                dt.Rows.Add(p.ID, p.Name, p.Price, p.Quantity);
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Grid.xlxs");
                }
            }
        }

    }
}