using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FFPark.Core.Result;
using FFPark.Entity.Park;
using FFPark.Services.Park;
using FFPark.Web.Framework.Controllers;
namespace FFPark.WebAPI.Controllers
{
    /// <summary>
    /// 小程序管理-通用广告
    /// </summary>
    [ApiController]
    [Route("api/wx/[action]")]
    [ApiExplorerSettings(GroupName = "pc")]

    public class WxAdCurrencyPresentationController : BaseAuthorizeController
    {
        private readonly IAdCurrencyPresentationServices _services;
        public WxAdCurrencyPresentationController(IAdCurrencyPresentationServices services)
        {
            _services = services;
        }
        /// <summary>
        /// 获取通用广告
        /// </summary>
        /// <param name="name">广告位置</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel<List<AdCurrencyPresentation>>> GetAdCurrency(string name = "首页顶部")
        {
            var result = await _services.GetAdCurrencyPresentationByName(name);
            return Success(result);
        }
    }
}
