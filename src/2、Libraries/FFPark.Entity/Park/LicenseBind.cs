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
    /// 车牌绑定
    /// </summary>
    [Table(Name = "LicenseBind")]
    public class LicenseBind:BaseEntity
    {
        /// <summary>
        /// 车牌号码
        /// </summary>
        public string License { get; set; }
        /// <summary>
        /// 小程序OpenId
        /// </summary>
        public string SmallProgramOpenId { get; set; }
    }
}
