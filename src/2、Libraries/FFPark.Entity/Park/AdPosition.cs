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
    /// 广告位置
    /// </summary>
    [Table(Name = "AdPosition")]
    public class AdPosition : BaseEntity
    {
        /// <summary>
        /// 广告位置名称
        /// </summary>
        public string PositionName { get; set; }
    }
}
