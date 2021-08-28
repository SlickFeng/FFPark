using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFPark.Core;
namespace FFPark.Entity.WeChat
{

    [Table(Name = "WebChatNotice")]
    public class WebChatNotice : BaseEntity
    {
        public string Title { get; set; }
        public string TemplateId { get; set; }
        public string TemplateContent { get; set; }
        public string Example { get; set; }

        public int TemplateType { get; set; }
    }
}
