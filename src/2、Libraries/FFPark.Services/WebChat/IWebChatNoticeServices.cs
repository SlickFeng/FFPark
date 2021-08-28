using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFPark.Entity.WeChat;
namespace FFPark.Services.WebChat
{
    public interface IWebChatNoticeServices
    {
        Task<List<WebChatNotice>> GetWebChatNoticeByTitle(List<string> title);

        Task<bool> AddWebChatNotice(IList<WebChatNotice> model);


    }
}
