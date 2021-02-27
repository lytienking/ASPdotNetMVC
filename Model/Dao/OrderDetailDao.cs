using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.ViewModel;

namespace Model.Dao
{
    public class OrderDetailDao
    {
        OnlineShopDbContext db = null;
        public OrderDetailDao()
        {
            db = new OnlineShopDbContext();
        }
        public bool Insert(OrderDetail detail)
        {
            try
            {
                db.OrderDetails.Add(detail);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
            
        }
        public OrderDetailViewModel OrderDetail(long orderdetailID)
        {
            var orderDetail = db.Orders.Where(x => x.ID == orderdetailID).
                Join(db.OrderDetails, order => order.ID, orderDetails => orderDetails.OrderID, (order, orderDetails) => new { order, orderDetails }).
                Join(db.Products, bill => bill.orderDetails.ProductID, product => product.ID, (bill, product) => new { bill, product });
            var detailModel = new OrderDetailViewModel();
            foreach(var item in orderDetail)
            {
                detailModel.items.Add(new ItemOrderDetail { product = item.product, quantity = item.bill.orderDetails.Quantity, size = item.bill.orderDetails.Size });
                detailModel.TotalQuantity += item.bill.orderDetails.Quantity;
                detailModel.TotalPrice += item.bill.orderDetails.Quantity * item.bill.orderDetails.Price;
                detailModel.customerID = item.bill.order.CustomerID;
                detailModel.orderID = item.bill.order.ID;
                detailModel.status = item.bill.order.Status;
            }
            detailModel.user = db.Users.SingleOrDefault(x => x.ID == detailModel.customerID);
            return detailModel;
        }
        public bool Delete(int id,int orderid)
        {
            try
            {
                var orderdetail = db.OrderDetails.Find(id,orderid);
                db.OrderDetails.Remove(orderdetail);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}
