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
    /// 通用广告位--停车场
    /// </summary>
    [Table(Name = "AdCurrencyPresentation")]
    public class AdCurrencyPresentation : BaseEntity
    {
        /// <summary>
        /// 广告展示位置
        /// </summary>
        public string LayerName { get; set; }
        /// <summary>
        /// 广告图片地址
        /// </summary>
        public string AdImageUrl { get; set; }
        /// <summary>
        /// 广告名称
        /// </summary>
        public string AdName { get; set; }
        /// <summary>
        /// 跳转Http 地址
        /// </summary>
        public string TargetHttpUrl { get; set; }
        /// <summary>
        /// 广告位跳转小程序地址
        /// </summary>
        public string SmallProgramTargetUrl { get; set; }


        public string Remark { get; set; }

        /// <summary>
        /// 广告位置Id
        /// </summary>
        public int AdPositionId { get; set; }
    }
}
