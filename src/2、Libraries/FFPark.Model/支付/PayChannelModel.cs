using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFPark.Model
{
    public class PayChannelModel : BaseFFParkModel
    {
        /// <summary>
        /// 通道名称
        /// </summary>
        [Required(ErrorMessage = "支付通道名称不可为空")]
        public string ChannelName { get; set; }

        /// <summary>
        /// 通道全称
        /// </summary>
        [Required(ErrorMessage = "通道全称不可为空")]
        public string ChannelFullName { get; set; }

        /// <summary>
        /// 通道备注
        /// </summary>
        public string Remark { get; set; }
    }

    public class PayChannelModelWithId : PayChannelModel
    {
        [Required(ErrorMessage = "Id 不可为空")]
        public override int Id { get; set; }
    }

}
