using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using FFPark.Core.Extensions;
using FFPark.Core.Result;
using FFPark.Core.Sign;
namespace FFPark.Services.Common
{
    public class IgnoreSignAttribute : Attribute
    {

    }
    /// <summary>
    /// 验签中间件
    /// </summary>
    public class SignGlobalMiddleware
    {
        IApiResult _apiResult;
        private readonly RequestDelegate next;
        public SignGlobalMiddleware(RequestDelegate next, IApiResult apiResult)
        {
            this.next = next;
            _apiResult = apiResult;
        }
        public async Task Invoke(HttpContext context)
        {
            //忽略签名
            var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;
            if (endpoint != null)
            {
                if (endpoint.Metadata
                .Any(m => m is IgnoreSignAttribute))
                {

                    await next(context);
                    return;
                }
            }
            context.Request.EnableBuffering();
            context.Request.Body.Position = 0;
            StreamReader reader = new StreamReader(context.Request.Body, Encoding.UTF8);
            string str = reader.ReadToEndAsync().Result;

            var convertedDictionatry = context.Request.Query.ToDictionary(s => s.Key, s => s.Value);

            Dictionary<string, string> queryDic = str.ToBDictionary();

            if (queryDic.Count == 0)
            {
                await SignErrorAsync(context, ApiStatusCode.SignFail.ToEnumInt(), OperationMessage.SignErrorNoParamter);
            }
            else if (!queryDic.ContainsKey(VerifySignOption.Sign))
            {
                await SignErrorAsync(context, ApiStatusCode.SignFail.ToEnumInt(), OperationMessage.NoProvideSign);
            }
            else if (!queryDic.ContainsKey(VerifySignOption.TimeStamp))
            {
                await SignErrorAsync(context, ApiStatusCode.SignFail.ToEnumInt(), OperationMessage.NoProvideTimeStamp);
            }

            var query = HttpUtility.ParseQueryString(str);
            string sign = string.Empty;
            sign = queryDic[VerifySignOption.Sign];

            var timestampStr = query[VerifySignOption.TimeStamp];
            bool longtr = double.TryParse(timestampStr, out double timestamp);

            if (timestampStr.Length != 10)
            {
                await SignErrorAsync(context, ApiStatusCode.SignFail.ToEnumInt(), OperationMessage.TimeStampFormatError);
            }

            bool check = SignHelper.ValidateTimestamp(timestamp, 50);

            if (longtr && check)
            {
                //移除sign
                queryDic.Remove(VerifySignOption.Sign);
                //移除timestamp
                queryDic.Remove(VerifySignOption.TimeStamp);
                string textASCII = queryDic.ToASCII();
                //对ASCII 加签
                string newsign = SignHelper.GetSHA256Sign(textASCII, "miyao123456789");
                string json = JsonConvert.SerializeObject(queryDic);
                if (sign == newsign)
                {
                    await next(context);
                }
                else
                {
                    await SignErrorAsync(context, ApiStatusCode.SignFail.ToEnumInt(), OperationMessage.ParmterError);
                }
            }
            else
            {
                await SignErrorAsync(context, ApiStatusCode.SignFail.ToEnumInt(), "非法请求,请校准客户端时间后再试");
            }
        }
        //异常错误信息捕获，将错误信息用Json方式返回
        private static Task SignErrorAsync(HttpContext context, int statusCode, string message)
        {
            var result = JsonConvert.SerializeObject(new ApiResult() { status = statusCode, success = false, msg = message });
            context.Response.ContentType = "application/json;charset=utf-8";
            return context.Response.WriteAsync(result);
        }

    }

}
