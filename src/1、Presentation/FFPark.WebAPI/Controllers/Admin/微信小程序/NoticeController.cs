using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FFPark.Core.Result;
using FFPark.Entity.WeChat;
using FFPark.Services.WebChat;
using FFPark.Web.Framework.Controllers;

namespace FFPark.WebAPI.Controllers
{
    /// <summary>
    /// 小程序消息推送
    /// </summary>
    [ApiController]
    [Route("api/wx/[controller]/[action]")]
    public class NoticeController : BaseAuthorizeController
    {

        private readonly IWebChatNoticeServices _services;
        public NoticeController(IWebChatNoticeServices services)
        {
            _services = services;
        }
        /// <summary>
        /// 根据标题获取模板Id
        /// </summary>
        /// <param name="templateName"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<List<WebChatNotice>>> GetSubscription(string templateName = "预约取消通知,居民房屋安全预警提醒")
        {
            string[] strArr = templateName.Split(new char[] { ',' });

            List<string> listName = new List<string>(strArr);

            var result = await _services.GetWebChatNoticeByTitle(listName);

            return Success(result);

        }


    }
}
