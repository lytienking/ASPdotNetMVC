using Model.EF;
using Model.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{

    public class ProductDao
    {
        OnlineShopDbContext db = null;
        public ProductDao()
        {
            db = new OnlineShopDbContext();
        }
        public long InSert(Product entity)
        {
            db.Products.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }
        public bool Delete(int id)
        {
            try
            {
                var product = db.Products.Find(id);
                db.Products.Remove(product);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public bool Update(Product entity)
        {
            try
            {
                var product = db.Products.Find(entity.ID);
                product.Name = entity.Name;
                product.Code = entity.Code;
                product.MetaTitle = entity.MetaTitle;
                product.Image = entity.Image;
                product.Price = entity.Price;
                product.PromotionPrice = entity.PromotionPrice;
                product.Quantity = entity.Quantity;
                product.CategoryID = entity.CategoryID;
                product.Detail = entity.Detail;
                product.CreatedDate = entity.CreatedDate;
                product.Status = entity.Status;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        //Danh sách sản phẩm theo danh mục
        public List<Product> ListByCategoryID(long categoryID, ref int totalRecord, int pageIndex, int pageSize)
        {
            totalRecord = db.Products.Where(x => x.CategoryID == categoryID).Count();
            var model = db.Products.Where(x => x.CategoryID == categoryID).OrderBy(x => x.CreatedDate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return model;
        }
        public List<Product> Search(string keywword, ref int totalRecord, int pageIndex , int pageSize )
        {
            totalRecord = db.Products.Where(x => x.Name.Contains(keywword)).Count();
            var model = db.Products.Where(x => x.Name.Contains(keywword)).OrderBy(x => x.CreatedDate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return model;
        }
        //Danh sách tất cả sản phẩm
        public List<Product> ListProduct()
        {
            return db.Products.OrderByDescending(x => x.CreatedDate).ToList();
        }
        public List<string> ListName(string keyword)
        {
            return db.Products.Where(x => x.Name.Contains(keyword)).Select(x => x.Name).ToList();
        }
        public List<NewProductViewModel> ListNewProduct(int top)
        {
            var model = from a in db.Products
                        join b in db.ProductCategories
                        on a.CategoryID equals b.ID
                        select new NewProductViewModel()
                        {
                            ID = a.ID,
                            Name = a.Name,
                            Image = a.Image,
                            Price = a.Price,
                            PromotionPrice = a.PromotionPrice,
                            CategoryID = a.CategoryID,
                            CreatedDate = a.CreatedDate,
                            Status = a.Status,
                            ParentID = b.ParentID
                        };
            return model.Where(x => x.ParentID == 1).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
        }
        public List<HistoryViewModel> ListBuy()
        {
            var model = from a in db.OrderDetails
                        join b in db.Orders on a.OrderID equals b.ID
                        join c in db.Products on a.ProductID equals c.ID
                        select new HistoryViewModel()
                        {
                            ID = b.CustomerID,
                            Name = c.Name,
                            Image = c.Image,
                            Size = a.Size,
                            Price = a.Price,
                            Quantity = a.Quantity,
                            CreatedDate = b.CreatedDate

                        };
            return model.OrderBy(a => a.CreatedDate).ToList();
        }
        public List<NewProductViewModel> ListNewProduct2(int top)
        {
            var model = from a in db.Products
                        join b in db.ProductCategories
                        on a.CategoryID equals b.ID
                        select new NewProductViewModel()
                        {
                            ID = a.ID,
                            Name = a.Name,
                            Image = a.Image,
                            Price = a.Price,
                            PromotionPrice = a.PromotionPrice,
                            CategoryID = a.CategoryID,
                            CreatedDate = a.CreatedDate,
                            Status = a.Status,
                            ParentID = b.ParentID
                        };
            return model.Where(x => x.ParentID == 2).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
        }
        public Product ViewDetail(long id)
        {
            return db.Products.Find(id);
        }
        public IEnumerable<Product> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Product> model = db.Products;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString) || x.ID == (long.Parse)(searchString));
            }
            return model.OrderBy(x => x.ID).ToPagedList(page, pageSize);
        }
    }
}
