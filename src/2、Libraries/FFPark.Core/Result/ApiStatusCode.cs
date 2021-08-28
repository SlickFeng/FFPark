using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFPark.Core.Result
{
    /// <summary>
    /// API 状态码
    /// </summary>
    public enum ApiStatusCode
    {
        #region   以1 开头    Token 相关错误 代码
        /// <summary>
        /// 无效的token
        /// </summary>
        [Description("无效的token")]
        TokenInvalid = 10001,

        /// <summary>
        /// token 已过期
        /// </summary>
        [Description("token 已过期")]
        TokenExpire = 10002,


        /// <summary>
        /// 获取token 失败
        /// </summary>
        [Description("获取token 失败")]
        TokenFail = 10003,

        /// <summary>
        /// 获取token 成功
        /// </summary>
        [Description("获取token 成功")]
        TokenSuccess = 10004,

        /// <summary>
        /// token 未提供
        /// </summary>
        [Description("token 未提供")]
        TokenNoProvide = 10005,

        #endregion

        #region   以2 开头 参数级别错误代码
        /// <summary>
        /// 无效的参数
        /// </summary>
        [Description("无效的参数")]
        ParameterInvalid = 20001,
        #endregion

        #region   以3 开头 系统级别错误代码

        /// <summary>
        /// 系统错误
        /// </summary>
        [Description("系统错误")]
        SystemException = 30001,
        #endregion

        #region  数据请求
        /// <summary>
        /// 请求成功
        /// </summary>
        [Description("请求成功")]
        RequestSuccess = 200,

        /// <summary>
        /// 请求失败
        /// </summary>
        [Description("请求失败")]
        RequestError = 201,

        #endregion


        #region     权限相关
        /// <summary>
        /// 无访问权限
        /// </summary>
        [Description("无访问权限")]
        NoAccess = 40001,

        /// <summary>
        /// 该请求无访问权限
        /// </summary>
        [Description("该请求无访问权限")]
        UnAuthorization = 401,

        #endregion

        [Description("验签失败")]
        SignFail = 50001
    }
}
