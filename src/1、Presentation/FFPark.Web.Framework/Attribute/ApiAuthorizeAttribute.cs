using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FFPark.Core.Extensions;
using FFPark.Core.Result;

namespace FFPark.Web.Framework.Attributes
{
    public class ApiAuthorizeAttribute : ActionFilterAttribute
    {



        private List<string> GetClaimValueByType(IEnumerable<Claim> claims, string ClaimType)
        {
            return (from item in claims
                    where item.Type == ClaimType
                    select item.Value).ToList();
        }
        public string Roles { get; set; }

        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            ApiResultBase apiResult = new ApiResultBase();
            HttpRequest request = context.HttpContext.Request;
            string authorization = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            if (authorization.IsNullOrEmpty())
            {
                //返回未提供token 数据 
                context.Result = apiResult.SetError(ApiStatusCode.TokenNoProvide.ToEnumInt(), ApiStatusCode.TokenNoProvide.EnumDescription());
            }
            else
            {
                try
                {
                    JwtSecurityToken jwt;
                    var token = authorization.Replace("Bearer ", "");
                    jwt = new JwtSecurityToken(token);
                    //验证token 是否过期
                    if (jwt.ValidTo < DateTime.UtcNow.AddSeconds(10))
                    {
                        //token已过期                     
                        context.Result = apiResult.SetError(ApiStatusCode.TokenExpire.ToEnumInt(), ApiStatusCode.TokenExpire.EnumDescription());
                    }
                    else
                    {
                        string roleNames = GetClaimValueByType(jwt.Claims, "role").FirstOrDefault();
                        bool isRoles = roleNames.Contains(Roles);
                        if (!isRoles)
                            context.Result = apiResult.SetError(ApiStatusCode.NoAccess.ToEnumInt(), ApiStatusCode.NoAccess.EnumDescription());
                    }
                }
                catch (Exception)
                {
                    //无效的token
                    context.Result = apiResult.SetError(ApiStatusCode.TokenInvalid.ToEnumInt(), ApiStatusCode.TokenInvalid.EnumDescription());
                }
            }
            return base.OnActionExecutionAsync(context, next);
        }
    }
}
