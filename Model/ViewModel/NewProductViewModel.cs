using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class NewProductViewModel
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public decimal? Price { get; set; }
        public decimal? PromotionPrice { get; set; }
        public long? CategoryID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool Status { get; set; }
        public long? ParentID { get; set; }
    }
}
