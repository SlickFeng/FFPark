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
    /// 车主优惠券
    /// </summary>
    [Table(Name = "CarSpecialDiscount")]
    public class CarSpecialDiscount : BaseEntity
    {
        public int ParkId { get; set; }


        public string SmallOpenId { get; set; }

        public string CardNo { get; set; }

        public string LicenseNo { get; set; }

        public int DiscountTime { get; set; }

        public DateTime BeginTime { get; set; }

        public DateTime EndTime { get; set; }
    }
}
