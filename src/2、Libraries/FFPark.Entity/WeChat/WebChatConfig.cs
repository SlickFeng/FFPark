using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFPark.Core;
using FreeSql.DataAnnotations;

namespace FFPark.Entity.WeChat
{
    [Table(Name = "WebChatConfig")]
    public class WebChatConfig : BaseEntity
    {
        public string SmallAppId { get; set; }

        public string SmallAppsecret { get; set; }
    }
}
