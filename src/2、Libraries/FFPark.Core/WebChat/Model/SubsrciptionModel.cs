using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFPark.Core.WebChat.Model
{
    /// <summary>
    /// 微信消息推送固定参数
    /// </summary>
    public class SubsrciptionModel
    {
        /// <summary>
        /// 微信加密签名，signature结合了开发者填写的token参数和请求中的timestamp参数、nonce参数。
        /// </summary>
        public string Signature { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public string TimeStamp { get; set; }

        /// <summary>
        /// 随机数
        /// </summary>
        public string Nonce { get; set; }
        /// <summary>
        /// 随机字符串
        /// </summary>

        public string Echostr { get; set; }
    }
}
