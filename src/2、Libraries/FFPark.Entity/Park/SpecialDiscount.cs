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
    /// 停车场优惠券
    /// </summary>
    [Table(Name = "SpecialDiscount")]
    public class SpecialDiscount : BaseEntity
    {
        /// <summary>
        /// 优惠时间---单位小时
        /// </summary>
        public int DiscountTime { get; set; }
        /// <summary>
        /// 停车场Id
        /// </summary>
        public int ParkId { get; set; }
        /// <summary>
        /// 优惠券开始时间
        /// </summary>
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// 优惠券结束时间
        /// </summary>
        public DateTime EndTime { get; set; }


        /// <summary>
        /// 优惠券名称
        /// </summary>
        public string SpecialDiscountName { get; set; }

    }
}
