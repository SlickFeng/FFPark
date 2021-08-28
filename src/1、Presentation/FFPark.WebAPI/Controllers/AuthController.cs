using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using FFPark.Core.Extensions;
using FFPark.Core.Result;
using FFPark.Services.Base;
using FFPark.Web.Framework.Controllers;
using FFPark.Web.Framework.IdentityServer;
using FFPark.WebAPI.Models;
using FFPark.Services;

namespace FFPark.WebAPI.Controllers
{
    /// <summary>
    /// 授权接口
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(GroupName = "auth")]
    public class AuthController : APIBaseController
    {
        #region   字段
        private readonly IBaseUserServices _userServices;
        private readonly ITokenPermissionService _tokenService;
        private readonly IUserRoleServcies _userRoleServcies;
        private readonly ICodeFirstServices _services;
        #endregion


        private readonly ILogger<AuthController> _logger;
        public AuthController(
            ILogger<AuthController> logger,
            IBaseUserServices userServices,
            IUserRoleServcies userRoleServcies,
            ITokenPermissionService tokenService, ICodeFirstServices services)
        {
            _userServices = userServices;
            _userRoleServcies = userRoleServcies;
            _tokenService = tokenService;
            _logger = logger;
            _services = services;
        }


        /// <summary>
        /// 小程序授权个人信息登录
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        //[HttpPost]
        //public async Task<MessageModel<AuthTokenModel>> SmallAuthenticate(string code, string phone)
        //{
        //    //根据手机号码查询用户信息来登录
        //    //var user=await _userServices.SmallNoAuthenticate()
        //}


        /// <summary>
        /// 系统初始化
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel> SystemInitial()
        {
             _services.DBInitial();
            return SuccessMsg("注销成功");
        }


        /// <summary>
        /// PC 登录 获取token
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel<AuthTokenModel>> Authenticate(SignModel model)
        {
          
            //判断用户名密码是否通过非空,长度验证
            var result = await _userServices.FindByNameAsync(model.AppId.SafeString());
            if (result)
            {
                var user = await _userServices.Login(model.AppId, model.AppSecret);
                //用户名密码错误
                if (user == null)
                {
                    return FailedMsg<AuthTokenModel>("用户名/密码错误");
                }
                else
                {
                    //判断用户状态- 用户是否启用, 禁用 等                       
                    var response = await _tokenService.RequestTokenAsync(model.AppId, model.AppSecret);
                    if (response.IsError)
                    {
                        return FailedMsg<AuthTokenModel>("获取Token 失败" + response.HttpErrorReason);
                    }
                    else
                    {
                        AuthTokenModel tokenModel = new AuthTokenModel()
                        {
                            AccessToken = response.AccessToken,
                            RefreshToken = response.RefreshToken,
                            ExpiresIn = response.ExpiresIn
                        };
                        return Success(tokenModel);
                    }
                }
            }
            else
            {
                return FailedMsg<AuthTokenModel>("该用户不存在");
            }
        }

        /// <summary>
        /// 刷新Token
        /// </summary>
        /// <param name="refreshToken">refreshToken </param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel<AuthTokenModel>> RefreshToken(string refreshToken)
        {

            var response = await _tokenService.RequestTokenByRefreshToken(refreshToken);
            if (response.IsError)
            {
                return FailedMsg<AuthTokenModel>("获取Token 失败" + response.HttpErrorReason);
            }
            else
            {
                AuthTokenModel tokenModel = new AuthTokenModel()
                {
                    AccessToken = response.AccessToken,
                    RefreshToken = response.RefreshToken,
                    ExpiresIn = response.ExpiresIn
                };
                return Success<AuthTokenModel>(tokenModel);
            }
        }

        /// <summary>
        /// 注销token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel> LayOut(string token)
        {
            var result = await _tokenService.RevokeToken(token);
            if (!result.IsError)
            {
                return SuccessMsg("注销成功");
            }
            else
            {
                return FailedMsg("注销失败" + result.Error);
            }
        }




    }
}
