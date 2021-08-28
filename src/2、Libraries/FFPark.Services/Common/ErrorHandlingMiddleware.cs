using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFPark.Core.Result;

namespace FFPark.Services.Common
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                var statusCode = context.Response.StatusCode;
                if (ex is ArgumentException)
                {
                    if (ex.Message.Contains("JWT"))
                    {
                        statusCode = 201;
                        await HandleExceptionAsync(context, statusCode, "无效的Token");
                    }
                    else
                    {
                        await HandleExceptionAsync(context, statusCode, ex.Message);
                    }
                }

            }

            finally
            {
                var statusCode = context.Response.StatusCode;
                var msg = "";
                switch (statusCode)
                {
                    case 201:
                        msg = "无效的Token";
                        break;
                    case 401:
                        msg = "很抱歉，您无权访问该接口，请确保已经登录!";
                        break;
                    case 403:
                        msg = "很抱歉，您无权访问该接口，请联系管理员授权!";
                        break;
                    case 404:
                        msg = "未找到服务,Url 错误";
                        break;

                    case 405:
                        msg = "确认请求方式 post/get";
                        break;

                    case 429:
                        msg = "请求次数过快";
                        break;

                    case 500:
                        msg = "系统内部错误";
                        break;
                    case 502:
                        msg = "请求错误";
                        break;
                    case 503:
                        msg = "503错误";
                        break;

                    case 200:
                        break;
                }
                if (!string.IsNullOrWhiteSpace(msg))
                {
                    await HandleExceptionAsync(context, statusCode, msg);
                }
            }
        }

        //异常错误信息捕获，将错误信息用Json方式返回
        private static Task HandleExceptionAsync(HttpContext context, int statusCode, string message)
        {

            var result = JsonConvert.SerializeObject(new ApiResult() { status = statusCode, success = false, msg = message });
            context.Response.ContentType = "application/json;charset=utf-8";
            return context.Response.WriteAsync(result);
        }
    }
}
