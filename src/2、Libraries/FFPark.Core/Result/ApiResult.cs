using FFPark.Core.Extensions;

namespace FFPark.Core.Result
{
    public class ApiResult
    {
        public int status { get; set; } = 200;

        public bool success { get; set; } = true;
        public string msg { get; set; } = "";
        public object response { get; set; } = "";

        public virtual ApiResult SetOK(string msg = "")
        {
            this.msg = msg;
            return this;
        }
        public virtual ApiResult SetOK()
        {
            this.success = true;
            status = ApiStatusCode.RequestSuccess.ToEnumInt();
            return this;
        }


        public virtual ApiResult SetError(string msg, object data)
        {
            this.success = false;
            this.msg = msg;
            this.response = data;
            return this;
        }
        public virtual ApiResult SetOK(string msg, object data = null)
        {
            this.success = true;
            this.status = 200;
            this.response = data;
            this.msg = msg;
            return this;
        }

        public virtual ApiResult SetOk(int status = 200, string msg = "", object data = null)
        {
            this.success = true;
            this.msg = msg;
            this.response = data;
            this.status = status;
            return this;
        }
        public virtual ApiResult SetOK(int status, string msg, object data = null)
        {
            this.success = true;
            this.msg = msg;
            this.response = data;
            this.status = status;
            return this;
        }

        public virtual ApiResult SetOk(object data)
        {
            this.status = ApiStatusCode.RequestSuccess.ToEnumInt();
            this.success = true;
            this.response = data;
            return this;
        }
        public virtual ApiResult SetError(string msg, int status = 200)
        {
            this.success = false;
            this.msg = msg;
            this.status = status;
            return this;
        }
        public virtual ApiResult SetError(int status, string msg = "", object data = null)
        {
            this.success = false;
            this.msg = msg;
            this.response = data;
            this.status = status;
            return this;
        }

        /// <summary>
        /// 请求参数,格式不正确 返回消息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public virtual ApiResult SetErrorRequestParameter(object data = null)
        {
            return SetError(ApiStatusCode.ParameterInvalid.ToEnumInt(), ApiStatusCode.ParameterInvalid.ToDescriptionOrString(), data);
        }
    }
}
