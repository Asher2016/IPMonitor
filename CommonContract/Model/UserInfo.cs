using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CommonContract.Model
{
    [DataContract]
    [Serializable]
    public class UserInfo
    {
        [DataMember]
        public long SID { get; set; }

        [DataMember]
        [Required(ErrorMessage = "用户名必须填写")]
        [StringLength(10, MinimumLength = 3, ErrorMessage="用户名最小长度为3，最大长度为20.")]
        public string UserName { get; set; }

        [DataMember]
        [Required(ErrorMessage = "原始密码必须填写")]
        [StringLength(20, MinimumLength = 6, ErrorMessage="密码最小长度为6，最大长度为20.")]
        public string OldPassword { get; set; }

        [DataMember]
        [Required(ErrorMessage = "新密码必须填写")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "密码最小长度为6，最大长度为20.")]
        public string NewPassword { get; set; }

        [DataMember]
        [Required(ErrorMessage = "确认密码必须填写")]
        public string ConfirmPassword { get; set; }
    }
}
