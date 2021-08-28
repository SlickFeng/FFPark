using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFPark.Core.WebChat.Model
{
    public class NoticeTemplateModel
    {
        public string access_token { get; set; }

        /// <summary>
        /// 接收者 微信openid
        /// </summary>
        public string touser { get; set; }
    }
}
