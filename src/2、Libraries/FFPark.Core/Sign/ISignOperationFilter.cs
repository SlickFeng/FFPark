using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace FFPark.Core.Sign
{
   public partial  interface ISignOperationFilter
    {
        /// <summary>
        /// 获取AK/SK的Secret
        /// </summary>
        /// <param name="appid"></param>
        /// <returns></returns>
        public string GetSignSecret(string appid);

        /// <summary>
        /// 定义摘要算法
        /// </summary>
        /// <param name="message">待摘要的内容</param>
        /// <param name="secret">Ak/SK的secret</param>
        /// <param name="signMethod">客户端要求的加密方式;hmac，md5，hmac-sha256</param>
        /// <returns></returns>
        public string GetSignhHash(string message, string secret, EncryptEnum signMethod = EncryptEnum.Default);
    }
}
