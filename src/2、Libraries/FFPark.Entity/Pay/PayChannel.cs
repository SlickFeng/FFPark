using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFPark.Core;
using FreeSql.DataAnnotations;

namespace FFPark.Entity.Pay
{
    /// <summary>
    /// 支付通道
    /// </summary>
    [Table(Name = "PayChannel")]
    public class PayChannel : BaseEntity
    {
        /// <summary>
        /// 通道名称
        /// </summary>
        public string ChannelName { get; set; }

        /// <summary>
        /// 通道全程
        /// </summary>
        public string ChannelFullName { get; set; }

        /// <summary>
        /// 通道备注
        /// </summary>
        public string Remark { get; set; }

    }
}
