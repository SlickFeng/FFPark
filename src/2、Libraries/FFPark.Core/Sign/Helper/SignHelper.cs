using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FFPark.Core.Sign
{
    public class SignHelper
    {
        /// <summary>
        /// sha256
        /// </summary>
        /// <param name="message">消息体</param>
        /// <param name="secret">签名密钥</param>
        /// <returns></returns>
        public static string GetSHA256Sign(string message, string secret)
        {
            byte[] data = Encoding.UTF8.GetBytes(message + secret);
            SHA256 shaM = new SHA256Managed();
            var hashBytes = shaM.ComputeHash(data);
            var hexString = hashBytes.Aggregate(new StringBuilder(),
            (sb, v) => sb.Append(v.ToString("x2"))).ToString();
            return hexString;
        }
        /// <summary>
        /// HMACSHA256Sign 签名
        /// </summary>
        /// <param name="message">待签名消息体</param>
        /// <param name="secret">签名密钥</param>
        /// <returns></returns>
        internal static string GetHMACSHA256Sign(string message, string secret)
        {
            secret = secret ?? "";
            var encoding = new ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(message);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                var hexString = hashmessage.Aggregate(new StringBuilder(),
                (sb, v) => sb.Append(v.ToString("x2"))).ToString();
                return hexString;
            }
        }
        /// <summary>
        /// MD5获取签名
        /// </summary>
        /// <param name="message"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        internal static string CreateMD5(string message, string secret)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(message + secret));

                var hexString = hashBytes.Aggregate(new StringBuilder(),(sb, v) => sb.Append(v.ToString("x2"))).ToString();
                return hexString;
            }
        }
        internal static string GetUtf8(string unicodeString)
        {
            UTF8Encoding utf8 = new UTF8Encoding();
            Byte[] encodedBytes = utf8.GetBytes(unicodeString);
            String decodedString = utf8.GetString(encodedBytes);
            return decodedString;
        }
        /// <summary>
        /// 异步读取信息流数据
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        private static async Task<string> ReadStreamAsync(Stream stream, Encoding encoding)
        {
            using (StreamReader sr = new StreamReader(stream, encoding, true, 1024, true))
            {
                var str = await sr.ReadToEndAsync();
                stream.Seek(0, SeekOrigin.Begin);
                return str;
            }
        }

        internal static Encoding GetRequestEncoding(HttpRequest request)
        {
            var requestContentType = request.ContentType;
            var requestMediaType = requestContentType == null ? default(MediaType) : new MediaType(requestContentType);
            var requestEncoding = requestMediaType.Encoding;
            if (requestEncoding == null)
            {
                requestEncoding = Encoding.UTF8;
            }
            return requestEncoding;
        }

        internal static void EnableRewind(HttpRequest request)
        {
            if (!request.Body.CanSeek)
            {
                request.EnableBuffering();
            }
            if (!request.Body.CanSeek)
            {
                request.EnableBuffering();
            }
            request.Body.Seek(0L, SeekOrigin.Begin);
        }
        internal static async Task<string> ReadAsStringAsync(HttpContext context)
        {
            try
            {
                if (context.Request.ContentLength > 0)
                {
                    EnableRewind(context.Request);
                    var encoding = GetRequestEncoding(context.Request);
                    return await ReadStreamAsync(context.Request.Body, encoding);
                }
                return null;
            }
            catch (Exception ex) when (!ex.Message?.Replace(" ", string.Empty).ToLower().Contains("unexpectedendofrequestcontent") ?? true)
            {
                Console.WriteLine($"[ReadAsString] sign签名读取body出错");
                return null;
            }
        }

        internal static void BuildErrorJson(ActionExecutingContext context, string msg = "签名失败")
        {
            if (!context.HttpContext?.Response.HasStarted ?? false)
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                context.HttpContext.Response.ContentType = "application/json";
            }
            context.Result = new BadRequestObjectResult(msg);
        }


        private static double GetTimestamp(int format, DateTimeKind timeKind)
        {
            TimeSpan timeSpan = new TimeSpan();

            switch (timeKind)
            {
                case DateTimeKind.Utc: timeSpan = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, timeKind); break;
                case DateTimeKind.Local: timeSpan = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, timeKind); break;
                default: throw new Exception("时间类型 只能为 Local、Utc");
            }
            switch (format)
            {
                case 10: return timeSpan.TotalSeconds;
                case 13: return timeSpan.TotalMilliseconds;
                default: throw new Exception("时间戳格式 只能为 10、13");
            }
        }

        /// <summary>
        /// 获取10位时间戳
        /// </summary>
        /// <param name="timeKind">时间类型（只能为 Local、Utc，默认 Local）</param>
        /// <returns></returns>
        public static int Get10Timestamp(DateTimeKind timeKind = DateTimeKind.Local)
        {
            return Convert.ToInt32(GetTimestamp(10, timeKind));
        }

        /// <summary>
        /// 获取13位时间戳
        /// </summary>
        /// <param name="timeKind">时间类型（只能为 Local、Utc，默认 Local）</param>
        /// <returns></returns>
        public static long Get13Timestamp(DateTimeKind timeKind = DateTimeKind.Local)
        {
            return Convert.ToInt64(GetTimestamp(13, timeKind));
        }

        // <summary>
        /// 验证时间戳（10位、13位皆可）
        /// </summary>
        /// <param name="timestamp">时间戳</param>
        /// <param name="timeDiff">允许时差（10位时单位为 秒，13位时单位为 毫秒）</param>
        /// <param name="timeKind">时间类型（只能为 Local、Utc，默认 Local）</param>
        /// <returns></returns>
        public static bool ValidateTimestamp(double timestamp, int timeDiff, DateTimeKind timeKind = DateTimeKind.Local)
        {
            TimeSpan timeSpan = new TimeSpan();

            switch (timeKind)
            {
                case DateTimeKind.Utc: timeSpan = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, timeKind); break;
                case DateTimeKind.Local: timeSpan = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, timeKind); break;
                default: throw new Exception("时间类型 只能为 Local、Utc");
            }

            double nowTimestamp = 0;  //现在的时间戳
            int format = timestamp.ToString("f0").Length;

            switch (format)
            {
                case 10: nowTimestamp = timeSpan.TotalSeconds; break;
                case 13: nowTimestamp = timeSpan.TotalMilliseconds; break;
                default: throw new Exception("时间戳格式 错误");
            }

            double nowTimeDiff = nowTimestamp - timestamp;  //现在的时差

            if (-timeDiff <= nowTimeDiff && nowTimeDiff <= timeDiff)
                return true;
            else
                return false;
        }
    }
}
