using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFPark.Model
{
    /// <summary>
    /// 广告位置
    /// </summary>
    public class AdPositionModel : BaseFFParkModel
    {

        /// <summary>
        /// 广告位置名称
        /// </summary>
        [Required(ErrorMessage = "广告位置名称不可为为空")]
        [StringLength(20, ErrorMessage = "广告位置名称长度不得超过24位")]
        public string PositionName { get; set; }


    }
}
