using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFPark.Core.Result
{
    public enum ResponseEnum
    {
        /// <summary>
        /// 未知错误
        /// </summary>
        [Description("未知错误")]
        UnKnown = -1,
        /// <summary>
        /// 接口错误
        /// </summary>
        [Description("接口错误")]
        ApiError = -2,
        /// <summary>
        /// 成功
        /// </summary>
        [Description("成功")]
        Success = 200,
        /// <summary>
        /// 无权限
        /// </summary>
        [Description("无权限")]
        NoPermissions = 401,
        /// <summary>
        /// 禁止访问
        /// </summary>
        [Description("禁止访问")]
        Forbidden = 403,
        /// <summary>
        /// 找不到指定资源
        /// </summary>
        [Description("找不到指定资源")]
        NoFound = 404,
        /// <summary>
        /// 服务方法错误
        /// </summary>
        [Description("服务方法错误")]
        ServerError = 500,

        /// <summary>
        /// 重复执行错误
        /// </summary>
        [Description("重复执行")]
        RepeatError = 501
    }
}
