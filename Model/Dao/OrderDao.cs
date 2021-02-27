using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class OrderDao
    {
        OnlineShopDbContext db = null;
        private int status = 0;
        public OrderDao()
        {
            db = new OnlineShopDbContext();
        }
        public long Insert(Order order)
        {
            db.Orders.Add(order);
            db.SaveChanges();
            return order.ID;
        }
        public bool Update(Order entity)
        {
            try
            {
                var user = GetCode(entity.Code);
                user.Status = 2;
                user.Code = entity.Code;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public IEnumerable<Order> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Order> model = db.Orders;
            //if (!string.IsNullOrEmpty(searchString))
            //{
            //    model = model.Where(x => x.ShipName.Contains(searchString));
            //}
            //return model.OrderBy(x => x.ID).ToPagedList(page, pageSize);
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.ShipName.Contains(searchString));
            }
            if (this.status > 0)
                return model.Where(x => x.Status == this.status).OrderByDescending(x => x.ID).ToPagedList(page, pageSize);
            return model.OrderByDescending(x => x.ID).ToPagedList(page, pageSize);
        }
        public void changeStatus(int bill_id, int status)
        {
            var res = this.db.Orders.SingleOrDefault(x => x.ID == bill_id);
            res.Status = status;
            this.db.SaveChanges();
        }
        public Order GetCode(string Code)
        {
            return db.Orders.SingleOrDefault(x => x.Code == Code);
        }
        public bool Delete(int id)
        {
            try
            {
                var order = db.Orders.Find(id);
                db.Orders.Remove(order);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public OrderDao viaStatus(int status)
        {
            this.status = status;
            return this;
        }

    }
}
