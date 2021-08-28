using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFPark.Entity.Park
{
    /// <summary>
    /// 基础统计
    /// </summary>
   public class BaseStatistics
    {
        /// <summary>
        /// 已合作的停车场
        /// </summary>
        public int ParkCount { get; set; }

        /// <summary>
        /// 交易笔数
        /// </summary>
        public int TranCount { get; set; }

        /// <summary>
        /// 交易总金额
        /// </summary>
        public decimal TranAmount{ get; set; }
    }
}
