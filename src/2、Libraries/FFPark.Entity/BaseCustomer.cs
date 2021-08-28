using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFPark.Core;
using FreeSql.DataAnnotations;

namespace FFPark.Entity
{
    /// <summary>
    /// 客户表
    /// </summary>
    [Table(Name = "BaseCustomer")]
    public class BaseCustomer : BaseEntity
    {
        /// <summary>
        ///  性别  0 男 1 女
        /// </summary>
        public int WebChatSex { get; set; }
        /// <summary>
        /// 微信小程序openid
        /// </summary>
        public string SmallOpenId { get; set; }

        /// <summary>
        /// 10 房东  -10 租客
        /// </summary>
        public int IsType { get; set; }

        /// <summary>
        /// 是否激活  
        /// </summary>
        public bool IsActive => true;
    }
}
