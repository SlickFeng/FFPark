using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFPark.Core.Result;

namespace FFPark.Web.Framework.Controllers
{
    public class APIBaseController : Controller
    {

        /// <summary>
        /// 执行成功（返回数据对象）
        /// </summary>
        /// <typeparam name="T">数据对象</typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        [NonAction]
        protected MessageModel<T> Success<T>(T data)
        {
            return new MessageModel<T>()
            {
                response = data,
                msg = "请求成功",
                status = ResponseEnum.Success
            };
        }

        /// <summary>
        /// 执行成功返回数据对象,无泛型
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [NonAction]
        protected MessageModel SuccessData(object data)
        {
            return new MessageModel()
            {
                response = data,
                msg = "请求成功",
                status = ResponseEnum.Success
            };
        }
    

        /// <summary>
        /// 执行错误（返回数据对象 NULL）
        /// </summary>
        /// <typeparam name="T">成功时的错误对象，异常时数据对象返回null</typeparam>
        /// <param name="msg"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [NonAction]
        protected MessageModel<T> FailedMsg<T>(string msg = "服务器错误", ResponseEnum status = ResponseEnum.ServerError)
        {
            return new MessageModel<T>(status, msg);
        }

        /// <summary>
        /// 执行错误（无返回数据对象）
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [NonAction]
        protected MessageModel FailedMsg(string msg = "服务器错误", ResponseEnum status = ResponseEnum.ServerError)
        {
            return new MessageModel(status, msg);
        }
        /// <summary>
        /// 执行成功（无返回数据对象）
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        [NonAction]
        protected MessageModel SuccessMsg(string msg = "执行成功")
        {
            return new MessageModel(ResponseEnum.Success, msg);
        }
        /// <summary>
        /// 执行成功（可能用不到）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg"></param>
        /// <returns></returns>
        [NonAction]
        protected MessageModel<T> SuccessMsg<T>(string msg = "执行成功")
        {
            return new MessageModel<T>(ResponseEnum.Success, msg);
        }
    }
}
