using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFPark.Data;
using FFPark.Entity.WeChat;

namespace FFPark.Services.WebChat
{
    public class WebChatConfigServices : IWebChatConfigServices
    {
        private readonly IRepository<WebChatConfig> _services;


        public WebChatConfigServices(IRepository<WebChatConfig> services)
        {
            _services = services;
        }
        public async Task<WebChatConfig> GetWebChatConfig()
        {
            var result = await _services.GetAsync(t => t.IsDeleted == false && t.Id == 12);
            return result?.FirstOrDefault();
        }
    }
}
