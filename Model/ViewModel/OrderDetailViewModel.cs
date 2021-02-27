using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;

namespace Model.ViewModel
{
    public class OrderDetailViewModel
    {
        public OrderDetailViewModel()
        {
            items = new List<ItemOrderDetail>();
            TotalPrice = 0;
            TotalQuantity = 0;
        }
        public decimal? TotalPrice { get; set; }
        public int? TotalQuantity { get; set; }
        public List<ItemOrderDetail> items { get; set; }
        public User user { get; set; }
        public long? customerID { get; set; }
        public long? orderID { get; set; }
        public int status { get; set; }
    }
    public class ItemOrderDetail
    {
        public Product product { get; set; }
        public string size { get; set; }
        public int? quantity { get; set; }
    }
}
