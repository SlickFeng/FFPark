using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFPark.Core.Result
{
    public class ApiResultBase : IApiResult
    {
        ApiResult apiResult;

        public ApiResultBase()
        {
            apiResult = new ApiResult();
        }

        public JsonResult SetError(string message)
        {
            apiResult.SetError(message);
            return new JsonResult(apiResult);
        }

        public JsonResult SetError(string message, object data)
        {
            return new JsonResult(message, data);
        }

        public JsonResult SetError(int code, string message, object data)
        {
            apiResult.SetError(code, message, data);
            return new JsonResult(apiResult);
        }

        public JsonResult SetError(int code, string message)
        {
            apiResult.SetError(code, message);
            return new JsonResult(apiResult);
        }
        public JsonResult SetOK(string message)
        {
            apiResult.SetOK(message);
            return new JsonResult(apiResult);
        }

        public JsonResult SetOK(string message, object data)
        {
            apiResult.SetOK(message, data);
            return new JsonResult(apiResult);
        }

        public JsonResult SetOK(int code, string message, object data)
        {
            apiResult.SetOK(code, message, data);
            return new JsonResult(apiResult);
        }

        public JsonResult SetOK(int code, string message)
        {
            apiResult.SetOK(code, message);
            return new JsonResult(apiResult);
        }


    }
}
