using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFPark.Core.Extensions;

namespace FFPark.Core.Result
{
    /// <summary>
    /// 通用返回信息类
    /// </summary>
    public class MessageModel<T>
    {
        public MessageModel()
        {

        }
        public MessageModel(ResponseEnum Status, string Msg)
        {
            status = Status;
            msg = Msg;
        }
        /// <summary>
        /// 状态码
        /// </summary>
        public ResponseEnum status { get; set; } = ResponseEnum.ServerError;
        /// <summary>
        /// 操作是否成功
        /// </summary>
        public bool success => status == ResponseEnum.Success;
        /// <summary>
        /// 返回信息
        /// </summary>
        public string msg { get; set; } = ResponseEnum.ServerError.EnumDescription();
        /// <summary>
        /// 返回数据集合
        /// </summary>
        public T response { get; set; }

    }

    public class MessageModel
    {
        public MessageModel()
        {

        }
        public MessageModel(ResponseEnum Status, string Msg)
        {
            status = Status;
            msg = Msg;
        }
        /// <summary>
        /// 状态码
        /// </summary>
        public ResponseEnum status { get; set; } = ResponseEnum.ServerError;
        /// <summary>
        /// 操作是否成功
        /// </summary>
        public bool success => status == ResponseEnum.Success;
        /// <summary>
        /// 返回信息
        /// </summary>
        public string msg { get; set; } = ResponseEnum.ServerError.EnumDescription();
        /// <summary>
        /// 返回数据集合
        /// </summary>
        public object response { get; set; }

    }
}
