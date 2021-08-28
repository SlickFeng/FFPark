using FFPark.Core.Result;
using FFPark.Entity.Park;
using FFPark.Web.Framework.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FFPark.WebAPI.Controllers.Admin
{
    [ApiController]
    [Route("api/workplace/[action]")]
    //[ApiAuthorize(Roles = "SystemAdmin")]
    [ApiExplorerSettings(GroupName = "pc")]
    public class WorkPlaceController : BaseAuthorizeController
    {
        /// <summary>
        /// 基础统计
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<BaseStatistics>> Statistics()
        {
            BaseStatistics baseStatistics = new BaseStatistics()
            {
                ParkCount = 26,
                TranAmount = 1237723,
                TranCount = 9876
            };
            return Success(baseStatistics);
        }
    }
}
