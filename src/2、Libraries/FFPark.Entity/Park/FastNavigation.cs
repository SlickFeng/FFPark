using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFPark.Core;
using FreeSql.DataAnnotations;
namespace FFPark.Entity.Park
{
    /// <summary>
    /// 快捷导航
    /// </summary>
    [Table(Name = "FastNavigation")]
    public class FastNavigation : BaseEntity
    {
        /// <summary>
        /// 跳转路径
        /// </summary>
        public string NavigationUrl { get; set; }
        /// <summary>
        /// 提示
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public int UserId { get; set; }
    }
}
