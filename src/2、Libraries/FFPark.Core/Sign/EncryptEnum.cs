using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFPark.Core.Sign
{
    /// <summary>
    /// 签名加密
    /// </summary>
    [Flags]
    public enum EncryptEnum
    {
        /// <summary>
        /// Dafault 默认（签名默认HMACSHA256；脱敏默认不加密）
        /// </summary>
        Default = 0,

        /// <summary>
        /// 签名SHA256算法
        /// <remarks></remarks>
        /// </summary>
        SignSHA256 = 1,

        /// <summary>
        /// 签名MD5算法
        /// <remarks>
        /// </remarks>
        /// </summary>
        /// <remarks>
        /// <![CDATA[ 1<<1]]>
        /// </remarks>
        SignMD5 = 2,

        /// <summary>
        /// 脱敏AES
        /// <remarks></remarks>
        /// </summary>
        /// <remarks> <![CDATA[ 1<<2]]></remarks>
        SymmetricAES = 4,

        /// <summary>
        /// 脱敏DES
        /// <remarks> <![CDATA[ 1<<3]]></remarks>
        /// </summary>
        SymmetricDES = 8,

        /// <summary>
        /// 脱敏Base64
        /// <remarks> <![CDATA[ 1<<4]]></remarks>
        /// </summary>
        /// <remarks>2^2;2^3</remarks>
        SymmetricBase64 = 16,

        /// <summary>
        /// 签名HMACSHA256算法
        /// <remarks> <![CDATA[ 1<<5]]></remarks>
        /// </summary>
        SignHMACSHA256 = 32,
    }
}
