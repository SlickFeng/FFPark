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
using FFPark.Web.Framework.Context;

namespace FFPark.Web.Framework.Controllers
{
    public class BaseAuthorizeController : APIBaseController
    {
        //传输过来的数据解析
        public WorkContext WorkContext;
        public BaseAuthorizeController()
        {
            WorkContext = new WorkContext();
        }
        /// <summary>
        /// 请求action 之后执行
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuted(ActionExecutedContext context)
        {

        }
        private List<string> GetClaimValueByType(IEnumerable<Claim> claims, string ClaimType)
        {
            return (from item in claims
                    where item.Type == ClaimType
                    select item.Value).ToList();
        }
        /// <summary>
        /// 请求action前执行 --只做解析
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string authorization = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrEmpty(authorization))
            {
                context.Result = new ApiResultBase().SetError(ApiStatusCode.TokenNoProvide.ToEnumInt(), ApiStatusCode.TokenNoProvide.ToDescriptionOrString());
                return;
            }
            JwtSecurityToken jwt;

            var token = authorization.Replace("Bearer ", "");
            jwt = new JwtSecurityToken(token);

            var claims = new List<Claim>();

            var userId = jwt.Claims.Where(t => t.Type == JwtRegisteredClaimNames.Sub).FirstOrDefault().Value;

            var roleNames = jwt.Claims.Where(t => t.Type == "role").FirstOrDefault().Value;

            //用户名
            string userName = GetClaimValueByType(jwt.Claims, "userName").ToString();

            WorkContext.UserId = userId.ToInt();
            WorkContext.UserName = userName;

            WorkContext.Roles = roleNames;
            //使用工作上下文,将userid 信息返回
        }
    }
}
