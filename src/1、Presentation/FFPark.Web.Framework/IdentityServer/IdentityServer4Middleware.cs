using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using FFPark.Core.Extensions;
using FFPark.Core.Result;
namespace FFPark.Web.Framework.IdentityServer
{

    /// <summary>
    /// Token 验证中间件
    /// </summary>
    public class IdentityServer4Middleware
    {

        private readonly RequestDelegate _next;
        private IApiResult _apiResult;
        public IdentityServer4Middleware(RequestDelegate next, IApiResult apiResult)
        {
            _next = next;
            _apiResult = apiResult;
        }
        public async Task Invoke(HttpContext context)
        {

            
            string authorization = context.Request.Headers["Authorization"].FirstOrDefault();
            if (!authorization.IsNullOrEmpty())
            {
                TokenValidateAsync(context, authorization);
            }
            await _next(context);
        }

        //异常错误信息捕获，将错误信息用Json方式返回
        private void TokenValidateAsync(HttpContext context, string authorization)
        {

            context.Response.ContentType = "application/json;charset=utf-8";
            string resultReturn = string.Empty;
            if (authorization.IsNullOrEmpty())
            {
                //返回未提供token 数据
                var result = _apiResult.SetError(ApiStatusCode.TokenNoProvide.ToEnumInt(), ApiStatusCode.TokenNoProvide.EnumDescription());
                resultReturn = JsonConvert.SerializeObject(result);
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
                        resultReturn = JsonConvert.SerializeObject(_apiResult.SetOK(ApiStatusCode.TokenExpire.ToEnumInt(), ApiStatusCode.TokenExpire.EnumDescription()));
                    }
                }
                catch (Exception)
                {
                    //无效的token
                    resultReturn = JsonConvert.SerializeObject(_apiResult.SetError(ApiStatusCode.TokenInvalid.ToEnumInt(), ApiStatusCode.TokenInvalid.EnumDescription()));
                }
                context.Response.WriteAsync(resultReturn);
            }
        }
    }
}
