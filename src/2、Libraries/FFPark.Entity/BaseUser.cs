using System;
using FFPark.Core;
using System.Collections;
using System.Collections.Generic;
using FreeSql.DataAnnotations;

namespace FFPark.Entity
{
    [Table(Name = "BaseUser")]
    public class BaseUser : BaseEntity
    {
        public string UserName { get; set; }

        public string UserPassWord { get; set; }

        public string UserType { get; set; }

    }
    /// <summary>
    /// 用户拓展表
    /// </summary>
    [Table(Name = "UserExtension")]
    public class UserExtension : BaseEntity
    {
        /// <summary>
        /// 用户手机号码
        /// </summary>
        public string UserPhone { get; set; }
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string PetName { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>

        public int UserId { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        public string Avatar { get;set;}
    }
}
