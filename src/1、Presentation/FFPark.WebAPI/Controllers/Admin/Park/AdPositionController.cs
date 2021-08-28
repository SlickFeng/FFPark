using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FFPark.Core.Result;
using FFPark.Services.Park;
using FFPark.Web.Framework.Attributes;
using FFPark.Web.Framework.Controllers;
using FFPark.Model;
using FFPark.Entity.Park;
using FFPark.WebAPI.Infrastructure.Mapper.Extensions;

namespace FFPark.WebAPI.Controllers.Admin.Park
{
    /// <summary>
    /// 后台-通用广告位置
    /// </summary>
    [ApiController]
    [Route("api/ad/adposition/[action]")]
    [ApiAuthorize(Roles = "SystemAdmin")]
    [ApiExplorerSettings(GroupName = "pc")]
    public class AdPositionController : BaseAuthorizeController
    {
        private readonly IAdPositionServices _services;
        public AdPositionController(IAdPositionServices services)
        {
            _services = services;
        }

        /// <summary>
        /// 新增通用广告位置
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel> RaiseAdPosition([FromBody] AdPositionModel model)
        {
            var entity = model.ToEntity<AdPosition>();
            var isName = await _services.IsExistAdPositionName(model.PositionName);
            if (isName) return FailedMsg("已存在名称为：" + model.PositionName + "的广告位置");
            var result = await _services.AdPosition(entity);
            if (result)
                return SuccessMsg("操作成功");
            else
                return SuccessMsg("操作失败");
        }
    }
}
