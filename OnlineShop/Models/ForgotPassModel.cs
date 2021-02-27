using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineShop.Models
{
    public class ForgotPassModel
    {
        [Required(ErrorMessage = "yêu cầu nhập Username")]
        public string Username { get; set; }
        [Required(ErrorMessage = "yêu cầu nhập Gmail")]
        public string Gmail { get; set; }
    }
}