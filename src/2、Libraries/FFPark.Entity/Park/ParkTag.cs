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
    ///  车场标签表
    /// </summary>
    [Table(Name = "ParkTag")]
    public class ParkTag : BaseEntity
    {
        /// <summary>
        /// 车场ID
        /// </summary>
        public int ParkId { get; set; }
        /// <summary>
        /// 车场标签
        /// </summary>
        public string Title { get; set; }
    }
}
