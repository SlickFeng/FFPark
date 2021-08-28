using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FFPark.Core.Result;
using FFPark.Entity;
using FFPark.Model;
using FFPark.Model.User;
using FFPark.Services.Base;
using FFPark.Web.Framework.Attributes;
using FFPark.Web.Framework.Controllers;
using FFPark.WebAPI.Infrastructure.Mapper.Extensions;

namespace FFPark.WebAPI.Controllers.Admin
{
    /// <summary>
    /// 后台-用户管理
    /// </summary>
    [ApiController]
    [Route("api/sys/role/[action]")]
    [ApiAuthorize(Roles = "SystemAdmin")]
    [ApiExplorerSettings(GroupName = "pc")]
    public class AdminRoleController : BaseAuthorizeController
    {

        private readonly IBaseRoleServices _baseRoleServices;

        public AdminRoleController(IBaseRoleServices baseRoleServices)
        {
            _baseRoleServices = baseRoleServices;
        }
        /// <summary>
        /// 新增内部角色
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel> RaiseRole([FromBody] BaseRoleModel model)
        {
            var roleEntity = model.ToEntity<BaseRole>();

            var isExistRoleName = await _baseRoleServices.IsExistRoleName(model.RoleName, 0);
            if (isExistRoleName)
                return FailedMsg("已存在" + model.RoleName);

            var userExtension = model.ToEntity<UserExtension>();

            var result = await _baseRoleServices.InsertRole(roleEntity);

            if (result)
                return SuccessMsg("操作成功");
            else
                return FailedMsg("操作失败");
        }
        /// <summary>
        /// 修改内部角色
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel> KeepRole([FromBody] BaseRoleModel model)
        {
            if (model.Id == 0)
            {
                return FailedMsg("编号不可为空");
            }
            var roleEntity = model.ToEntity<BaseRole>();
            var isExistRoleName = await _baseRoleServices.IsExistRoleName(model.RoleName, model.Id);
            if (isExistRoleName)
                return FailedMsg("已存在" + model.RoleName);
            var userExtension = model.ToEntity<UserExtension>();
            var result = await _baseRoleServices.UpdateRole(roleEntity);
            if (result)
                return SuccessMsg("操作成功");
            else
                return FailedMsg("操作失败");
        }

        /// <summary>
        /// 角色列表
        /// </summary>
        /// <param name="pageModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel<PageModel<BaseRole>>> GetBaseRole(PageModelDto pageModel)
        {
            var result = await _baseRoleServices.GetBaseRole(pageModel);
            return Success(result);
        }




    }
}
