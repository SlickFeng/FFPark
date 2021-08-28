using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using FFPark.Core.Result;
using FFPark.Core.Extensions;
namespace FFPark.Web.Framework.Filter
{
    /// <summary>
    /// model 验证filter
    /// </summary>
    public class ModelActionFilter : ActionFilterAttribute, IActionFilter
    {

        public ModelActionFilter()
        {

        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string resultMessage = string.Empty;

            if (!context.ModelState.IsValid)
            {

                foreach (var item in context.ModelState)
                {

                    foreach (var error in item.Value.Errors)
                    {
                        if (!string.IsNullOrEmpty(error.ErrorMessage))
                        {
                            resultMessage += error.ErrorMessage + ",";
                        }
                    }
                }
                resultMessage = resultMessage.Substring(0, resultMessage.Length - 1);
                context.Result = new JsonResult(new ApiResult().SetError(ApiStatusCode.ParameterInvalid.ToEnumInt(), resultMessage, ""));
            }
        }
    }
}
