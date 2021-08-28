using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FFPark.Core.Result;
using FFPark.Core.WebChat;
using FFPark.Core.WebChat.Model;
using FFPark.Entity.WeChat;
using FFPark.Services.WebChat;
using FFPark.Web.Framework.Attributes;
using FFPark.Web.Framework.Controllers;

namespace FFPark.WebAPI.Controllers
{
    /// <summary>
    /// 微信小程序管理
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(GroupName = "pc")]
    public class WebChatNoticeController : BaseAuthorizeController
    {
        private readonly IWebChatNoticeServices _noticeServices;
        private readonly IWebChatConfigServices _configServices;

        private readonly HttpClient _httpClient;

        private readonly ISubscriptionMessageRequestHandler _handler;
        public WebChatNoticeController(
            IWebChatNoticeServices noticeServices,
            IWebChatConfigServices configServices,
            ISubscriptionMessageRequestHandler handler,

            HttpClient httpClient
            )
        {
            _noticeServices = noticeServices;
            _configServices = configServices;
            _handler = handler;
            _httpClient = httpClient;
        }


        /// <summary>
        /// 小程序同步
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel> ToAsyncSmallNotice()
        {
            //获取appid
            WebChatConfig config = await _configServices.GetWebChatConfig();

            var tokenMessage = await _handler.GetSmallAccesToken(config?.SmallAppId, config?.SmallAppsecret);

            var template = await _handler.GetSmallTemplate(tokenMessage?.access_token);

            if (template.errcode == 0)
            {
                List<WebChatNotice> list = new List<WebChatNotice>();
                foreach (var t in template.data)
                {
                    list.Add(new WebChatNotice()
                    {
                        TemplateContent = t.content,
                        Example = t.example,
                        TemplateId = t.priTmplId,
                        TemplateType = t.type,
                        Title = t.title
                    });
                }
                var result = await _noticeServices.AddWebChatNotice(list);
                if (result)
                    return SuccessMsg("操作成功");
                else
                    return SuccessMsg("操作失败");
            }
            return SuccessMsg("更新成功");
        }

        /// <summary>
        /// 微信小程序登录
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel> SmallProgramLogin(string code)
        {
            WebChatConfig config = await _configServices.GetWebChatConfig();
            var result = await _handler.GetSmallProgramOpenIdBySessionCode(code, config.SmallAppId, config.SmallAppsecret);
            return SuccessData(result.openid);
        }

        [HttpGet]
        public async Task<MessageModel> SenSubscription(string openid)
        {
            WebChatConfig config = await _configServices.GetWebChatConfig();
            var result = await _handler.GetSmallAccesToken(config.SmallAppId, config.SmallAppsecret);
            string openId = "o7fDx5OaCMWmKJb0Y1oSZ5krXOmw";
            string url = " http://wxopenapi.decerp.cc/api/WxOpen/WeChatGetUserOpenAccessToken?cappid=wx674f6d0501cbb6f1";
            string t = await _httpClient.GetStringAsync(url);
            //查询模板Id
            PlaceOrderSuccessNoticeModel model = new PlaceOrderSuccessNoticeModel()
            {
                access_token = t,
                touser = openid,
                OrderMoney = "100",
                OrderNo = "323423",
                PlaceOrderTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                ServiceItems = "测试项目",
                StoreName = "测试店铺",
                template_id = "QIG20Af69G4poqLuPKPxRnQvOJ6gl7BjlzVu8725A3o"
            };

            var send = await _handler.SmallPlaceOrderSuccessNotice(model);
            return SuccessData(send.errmsg);
        }
    }
}
