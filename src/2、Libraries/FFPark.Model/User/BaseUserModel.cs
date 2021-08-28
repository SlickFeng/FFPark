using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFPark.Model.User
{
    public class BaseUserModel : BaseFFParkModel
    {
        [Required(ErrorMessage = "用户名不可为空")]
        public string UserName { get; set; }

        /// <summary>
        /// 默认密码
        /// </summary>
        [Required(ErrorMessage = "必须填写初始密码")]
        public string Password { get; set; }

        /// <summary>
        /// 默认用户类型 1 系统管理用户 2  房东 3 租客
        /// </summary>
        public int UserType => 1;

        /// <summary>
        /// 手机号码
        /// </summary>
        public string UserPhone { get; set; }
        /// <summary>
        /// 用户昵称
        /// </summary> 
        public string PetName { get; set; }
    }

    public class BaseUserWithIdModel : BaseUserModel
    {
        [Required(ErrorMessage = "Id 不可为空")]
        public override int Id { get; set; }
    }


    public class BaseUserDto 
        {
        /// <summary>
        /// 用户昵称
        /// </summary> 
        public string PetName { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        public string Avatar { get; set; }
    }


}
