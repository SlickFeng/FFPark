using FFPark.Core.Result;
using FFPark.Entity.Park;
using FFPark.Model;
using FFPark.Services.Park;
using FFPark.Web.Framework.Attributes;
using FFPark.Web.Framework.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FFPark.WebAPI.Controllers.Admin.Park
{


    [ApiController]
    [Route("api/park/[action]")]
    //[ApiAuthorize(Roles = "SystemAdmin")]
    [ApiExplorerSettings(GroupName = "pc")]
    public class FastNavigationsController : BaseAuthorizeController
    {
        private readonly IFastNavigationServices _services;
        public FastNavigationsController(IFastNavigationServices services)
        {
            _services = services;
        }

        /// <summary>
        /// 获取当前用户的快捷导航设置
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<IList<FastNavigation>>> Navigations()
        {
            var result = await _services.GetFastNavigationsByUserId(WorkContext.UserId);
            return Success(result);
        }
    }
}
