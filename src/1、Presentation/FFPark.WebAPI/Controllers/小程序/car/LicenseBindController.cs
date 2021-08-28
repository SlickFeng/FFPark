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
    /// 车主小程序--车牌
    /// </summary>
    [ApiController]
    [Route("api/wx/[action]")]
    [ApiExplorerSettings(GroupName = "car")]
    public class LicenseBindController : BaseAuthorizeController
    {
        private readonly ILicenseBindServices _services;
        public LicenseBindController(ILicenseBindServices services)
        {
            _services = services;
        }

        /// <summary>
        /// 新增车牌绑定
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel> Bind(string license)
        {
            if (license.Length < 6)
            {
                return FailedMsg("车牌号错误");
            }
            LicenseBind entity = new LicenseBind()
            {
                License = license,
                //从token 中获取
                SmallProgramOpenId = "oTQRQ5Jt8rTOcbJsjxpCl7o1JlKg"
            };
            var list = await _services.GetLicenseByOpenId(entity.License, entity.SmallProgramOpenId);
            if (list?.Count() > 0)
                return FailedMsg("该车牌号已记录");
            var result = await _services.AddLicenseBind(entity);
            return SuccessMsg("操作成功");
        }

        /// <summary>
        /// 修改车牌绑定
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel> KeepBind(string license)
        {
            if (license.Length < 6)
            {
                return FailedMsg("车牌号错误");
            }
            LicenseBind entity = new LicenseBind()
            {
                License = license,
                //从token 中获取
                SmallProgramOpenId = "oTQRQ5Jt8rTOcbJsjxpCl7o1JlKg"
            };
            var result = await _services.UpdateLicenseBind(entity);
            return SuccessMsg("操作成功");
        }
    }
}
