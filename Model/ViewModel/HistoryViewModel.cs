using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class HistoryViewModel
    {
        public long? ID { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Size { get; set; }
        public decimal? Price { get; set; }
        public int? Quantity { get; set; }
        public DateTime? CreatedDate { get; set; }
        public decimal Total { get; set; }
    }
}
