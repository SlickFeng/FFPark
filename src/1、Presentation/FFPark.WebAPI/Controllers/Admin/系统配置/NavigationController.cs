using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using FFPark.Core.Result;
using FFPark.Entity;
using FFPark.Model;
using FFPark.Services.Base;
using FFPark.Web.Framework.Attributes;
using FFPark.Web.Framework.Controllers;
using FFPark.WebAPI.Infrastructure.Mapper.Extensions;

namespace FFPark.WebAPI.Controllers.Admin
{
    /// <summary>
    /// 后台-菜单管理
    /// </summary>
    [ApiController]
    [Route("api/sys/role/[action]")]
    [ApiAuthorize(Roles = "SystemAdmin")]
    [ApiExplorerSettings(GroupName = "pc")]
    public class NavigationController : BaseAuthorizeController
    {
        private readonly IBaseNavigationModuleServices _baseNavigationModuleServices;

        public NavigationController(IBaseNavigationModuleServices baseNavigationModuleServices)
        {
            _baseNavigationModuleServices = baseNavigationModuleServices;
        }
        /// <summary>
        /// 新增左侧导航菜单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel> RaiseNavigationModule([FromBody] BaseNavigationModuleModel model)
        {
            var entity = model.ToEntity<BaseNavigationModule>();

            var isExistName = await _baseNavigationModuleServices.IsExistTitle(model.Title, 0);
            if (isExistName)
                return FailedMsg("已存在" + model.Title);

            var result = await _baseNavigationModuleServices.Insert(entity);

            if (result)
                return SuccessMsg("操作成功");
            else
                return FailedMsg("操作失败");
        }
        /// <summary>
        /// 修改左侧菜单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel> KeepBaseNavigationModuleModel([FromBody] BaseNavigationModuleModel model)
        {
            if (model.Id == 0)
            {
                return FailedMsg("编号不可为空");
            }
            var roleEntity = model.ToEntity<BaseNavigationModule>();
            var isExistRoleName = await _baseNavigationModuleServices.IsExistTitle(model.Title, model.Id);
            if (isExistRoleName)
                return FailedMsg("已存在" + model.Title);
            var result = await _baseNavigationModuleServices.Update(roleEntity);
            if (result)
                return SuccessMsg("操作成功");
            else
                return FailedMsg("操作失败");
        }
        /// <summary>
        /// 左侧导航菜单列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel<List<BaseNavigationModule>>> GetNavigationModule()
        {
            var result =await _baseNavigationModuleServices.GetNavigationModule();
            return Success(result);
        }
    }
}
