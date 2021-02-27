using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineShop.Models
{
    public class CodeOrderModel
    {
        [Display(Name ="Mời bạn nhập code")]
        [Required(ErrorMessage = "yêu cầu nhập Code")]
        public string Code { get; set; }
    }
}