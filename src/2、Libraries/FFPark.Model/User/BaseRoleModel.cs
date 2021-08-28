using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFPark.Model.User
{
    /// <summary>
    /// 角色Model
    /// </summary>
    public class BaseRoleModel : BaseFFParkModel
    {
        [Required(ErrorMessage = "角色名不可为空")]
        public string RoleName { get; set; }

        /// <summary>
        /// 默认密码
        /// </summary>
        [Required(ErrorMessage = "角色描述不可为空")]
        public string RoleDescription { get; set; }

    }
}
