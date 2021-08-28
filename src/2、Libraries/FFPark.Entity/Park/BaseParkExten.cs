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
    /// 车场信息扩展表
    /// </summary>
    [Table(Name = "BaseParkExtend")]
    public class BaseParkExtend : BaseEntity
    {
        /// <summary>
        /// 车场Id
        /// </summary>
        public int ParkId { get; set; }

        /// <summary>
        /// 车场所在省份
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 车场所在城市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 车场所在区域
        /// </summary>
        public string Region { get; set; }
        /// <summary>
        /// 车场所在街道
        /// </summary>
        public string Street { get; set; }
        /// <summary>
        /// 车场详细地址
        /// </summary>
        public string ParkAddress { get; set; }
    }
}
