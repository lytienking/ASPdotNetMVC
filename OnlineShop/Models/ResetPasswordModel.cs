using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineShop.Models
{
    public class ResetPasswordModel
    {   
        [Required(ErrorMessage = "yêu cầu nhập Username")]
        public string Username { get; set; }
        [Required(ErrorMessage = "yêu cầu nhập Password")]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Nhập lại mật khẩu không đúng")]
        public string RePassword { get; set; }
        [Required(ErrorMessage = "yêu cầu nhập code")]
        public string Code { get; set; }
    }
}