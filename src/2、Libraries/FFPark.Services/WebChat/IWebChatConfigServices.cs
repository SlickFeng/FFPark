using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFPark.Entity.WeChat;
namespace FFPark.Services.WebChat
{
    public interface IWebChatConfigServices
    {
        Task<WebChatConfig> GetWebChatConfig();

    }
}
