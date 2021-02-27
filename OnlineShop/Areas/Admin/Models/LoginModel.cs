using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineShop.Areas.Admin.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "yêu cầu nhập Username")]
        public string Username { get; set; }
        [Required(ErrorMessage = "yêu cầu nhập Password")]
        public string Password { get; set; }
    }
}