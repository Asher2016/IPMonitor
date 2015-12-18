using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "用户不能为空")]
        [StringLength(20, ErrorMessage = "密码长度不能超过20个字符")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "密码不能为空")]
        [StringLength(20, ErrorMessage = "密码长度不能超过20个字符")]
        public string Password { get; set; }
    }
}