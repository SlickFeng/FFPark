using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFPark.Data;
using static IdentityModel.OidcConstants;
using FFPark.Entity;
using FFPark.Services.Base;
using IdentityModel;

namespace FFPark.Web.Framework.IdentityServer
{
    public class ResourceOwnerPasswordValidator<TUser> : IResourceOwnerPasswordValidator where TUser : class
    {

        private readonly ILogger<ResourceOwnerPasswordValidator<TUser>> _logger;

        private readonly IBaseUserServices _userServices;

        public ResourceOwnerPasswordValidator(

          ILogger<ResourceOwnerPasswordValidator<TUser>> logger, IBaseUserServices userServices)
        {
            _logger = logger;
            _userServices = userServices;
        }
        public virtual async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            //通过用户名获取用户信息
            bool isUser = await _userServices.FindByNameAsync(context.UserName);

            if (isUser)
            {
                var result = await _userServices.Login(context.UserName, context.Password);
                if (result != null)
                {
                    //subjectId 为用户唯一标识 一般为用户id
                    //authenticationMethod 描述自定义授权类型的认证方法 
                    //authTime 授权时间
                    //claims 需要返回的用户身份信息单元

                    context.Result = new GrantValidationResult(result.Id.ToString(), AuthenticationMethods.Password);
                    return;
                }
                else
                {
                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
                    return;
                }
            }
            else
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "用户不存在");
                return;
            }
        }
    }
}
