using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using FFPark.Core.Extensions;
using FFPark.Core.Result;
namespace FFPark.Web.Framework.Attribute
{
    public class ModelActionFilter : ActionFilterAttribute, IActionFilter
    {
        private IApiResult _apiResult;

        public ModelActionFilter()
        {
            _apiResult = new ApiResultBase();
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {

            if (!context.ModelState.IsValid)
            {
                var errorResults = new List<ErrorResult>();
                foreach (var item in context.ModelState)
                {
                    var result = new ErrorResult()
                    {
                        Field = item.Key,
                    };
                    foreach (var error in item.Value.Errors)
                    {
                        if (!string.IsNullOrEmpty(result.Message))
                        {
                            result.Message += "|";
                        }
                        result.Message += error.ErrorMessage;
                    }
                    errorResults.Add(result);
                }
                context.Result = _apiResult.SetError(ApiStatusCode.ParameterInvalid.ToEnumInt(), ApiStatusCode.ParameterInvalid.EnumDescription(), errorResults);
            }
        }
    }
    public class ErrorResult
    {
        /// <summary>
        /// 参数领域
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string Message { get; set; }
    }
}
