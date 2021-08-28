using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FFPark.Core.Result;
using FFPark.Entity;
using FFPark.Model.User;
using FFPark.Services.Base;
using FFPark.Web.Framework.Attributes;
using FFPark.Web.Framework.Controllers;
using FFPark.WebAPI.Infrastructure.Mapper.Extensions;
using FFPark.Core.Extensions;
namespace FFPark.WebAPI.Controllers.Admin
{
    /// <summary>
    /// 后台-用户管理
    /// </summary>
    [ApiController]
    [Route("api/sys/user/[action]")]
    [ApiAuthorize(Roles = "SystemAdmin")]
    [ApiExplorerSettings(GroupName = "pc")]
    /// </summary>
    public class AdminUserController : BaseAuthorizeController
    {
        private readonly IBaseUserServices _baseUserServices;
        private readonly IUserRoleServcies _userRoleServcies;
        public AdminUserController(IBaseUserServices baseUserServices, IUserRoleServcies userRoleServcies)
        {
            _baseUserServices = baseUserServices;
            _userRoleServcies = userRoleServcies;
        }
        /// <summary>
        /// 新增内部用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel> RaiseUser([FromBody] BaseUserModel model)
        {
            var userEntity = model.ToEntity<BaseUser>();
            var isExistPetName = await _baseUserServices.IsExistPetName(model.PetName, 0);
            if (isExistPetName)
                return FailedMsg("已存在" + model.PetName);
            userEntity.UserPassWord = model.Password;
            var userExtension = model.ToEntity<UserExtension>();
            var result = await _baseUserServices.InsertBaseUser(userEntity, userExtension);
            if (result)
                return SuccessMsg("操作成功");
            else
                return FailedMsg("操作失败");
        }
        /// <summary>
        /// 修改内部用户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel> KeepUser([FromBody] BaseUserWithIdModel model)
        {
            var userEntity = model.ToEntity<BaseUser>();
            var isExistPetName = await _baseUserServices.IsExistPetName(model.PetName, model.Id);
            if (isExistPetName)
                return FailedMsg("已存在" + model.PetName);
            var userExtension = model.ToEntity<UserExtension>();
            var result = await _baseUserServices.UpdateBaseUser(userEntity, userExtension);
            if (result)
                return SuccessMsg("操作成功");
            else
                return FailedMsg("操作失败");
        }

        [HttpGet]
        public async Task<MessageModel> SettingRole(int userId, string roles)
        {
            if (roles.StringToIntArray().Count() == 0)
                return FailedMsg("未提供角色信息");
            int[] roleArray = roles.StringToIntArray();
            var result = await _userRoleServcies.SettingRole(userId, roleArray);
            if (result)
                return SuccessMsg("操作成功");
            else
                return FailedMsg("操作失败");
        }
        /// <summary>
        /// 获取用户基本信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<UserExtension>> GetBaeUserInfo()
        {
            var result = await _baseUserServices.GetUserById(WorkContext.UserId);
            return Success(result);
        }
    }
}
