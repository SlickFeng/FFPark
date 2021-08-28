using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFPark.Core.Configuration
{
    /// <summary>
    /// 微信各种参数配置
    /// </summary>
    public class WebChatConfig:BaseEntity
    {
        public string AppId { get; set; }

        public string AppSecret { get; set; }
        /// <summary>
        /// 商户号
        /// </summary>
        public string MerchId { get; set; }
        public string Token { get; set; }
        public string EncodingAESKey { get; set; }
    }
}
