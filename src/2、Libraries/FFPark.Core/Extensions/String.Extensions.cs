using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFPark.Core.Extensions
{
    public static partial class Extensions
    {
        /// <summary>
        /// 安全获取值，当值为null时，不会抛出异常
        /// </summary>
        /// <param name="value">可空值</param>
        public static T SafeValue<T>(this T? value) where T : struct
        {
            return value ?? default(T);
        }

        public static Dictionary<string, string> ToBDictionary(this string str)
        {
            try
            {
                String[] dataArry = str.Split('&');
                Dictionary<string, string> dataDic = new Dictionary<string, string>();
                for (int i = 0; i <= dataArry.Length - 1; i++)
                {
                    String dataParm = dataArry[i];
                    int dIndex = dataParm.IndexOf("=");
                    if (dIndex != -1)
                    {
                        String key = dataParm.Substring(0, dIndex);
                        String value = dataParm.Substring(dIndex + 1, dataParm.Length - dIndex - 1);
                        dataDic.Add(key, value);
                    }
                }
                return dataDic;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 安全转换为字符串，去除两端空格，当值为null时返回""
        /// </summary>
        /// <param name="input">输入值</param>
        public static string SafeString(this object input)
        {
            return input == null ? string.Empty : input.ToString().Trim();
        }

        /// <summary>
        /// 字符串是否为空
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }
        public static string[] StringToStringArray(this string str)
        {
            if (str.IsNullOrEmpty())
                return new string[] { };
            //返回:{"1","2","3","4"} 不保留空元素
            string[] split = str.Split(new string[] { ",", "." }, StringSplitOptions.RemoveEmptyEntries);
            return split;
        }

        public static int[] StringToIntArray(this string str)
        {

            if (str.IsNullOrEmpty())
                return new int[] { };
            //返回:{"1","2","3","4"} 不保留空元素
            string[] sNums = str.Split(new string[] { ",", "." }, StringSplitOptions.RemoveEmptyEntries);

            int[] iNums = Array.ConvertAll(sNums, int.Parse);

            return iNums;
        }




        /// <summary>
        /// 字符串压缩
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Compress(this string input)
        {
            byte[] compressBeforeByte = Encoding.GetEncoding("UTF-8").GetBytes(input);
            byte[] compressAfterByte = compressBeforeByte.Compress();
            string compressString = Convert.ToBase64String(compressAfterByte);
            return compressString;
        }

        /// <summary>
        /// 字节流压缩
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] Compress(this byte[] data)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                GZipStream zip = new GZipStream(ms, CompressionMode.Compress, true);
                zip.Write(data, 0, data.Length);
                zip.Close();
                byte[] buffer = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(buffer, 0, buffer.Length);
                ms.Close();
                return buffer;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }

        /// <summary>
        /// 字符串解压
        /// </summary>
        public static string DecompressString(this string input)
        {
            string compressString = "";
            byte[] compressBeforeByte = Convert.FromBase64String(input);
            byte[] compressAfterByte = compressBeforeByte.Decompress();
            compressString = Encoding.GetEncoding("UTF-8").GetString(compressAfterByte);
            return compressString;
        }
        /// <summary>
        /// 字节流解压
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] Decompress(this byte[] data)
        {
            try
            {
                MemoryStream ms = new MemoryStream(data);
                GZipStream zip = new GZipStream(ms, CompressionMode.Decompress, true);
                MemoryStream msreader = new MemoryStream();
                byte[] buffer = new byte[0x1000];
                while (true)
                {
                    int reader = zip.Read(buffer, 0, buffer.Length);
                    if (reader <= 0)
                    {
                        break;
                    }
                    msreader.Write(buffer, 0, reader);
                }
                zip.Close();
                ms.Close();
                msreader.Position = 0;
                buffer = msreader.ToArray();
                msreader.Close();
                return buffer;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }
        public static string ToASCII(this Dictionary<string, string> paramsMap)
        {
            var vDic = (from objDic in paramsMap orderby objDic.Key ascending select objDic);
            StringBuilder str = new StringBuilder();
            foreach (KeyValuePair<string, string> kv in vDic)
            {
                string pkey = kv.Key;
                string pvalue = kv.Value;
                str.Append(pkey + "=" + pvalue + "&");
            }
            string result = str.ToString().Substring(0, str.ToString().Length - 1);
            return result;
        }

        /// <summary>
        /// 字符串转日期
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>

        public static DateTime ToDateTime(this string dateTime)
        {
            var result = DateTime.Parse(dateTime);
            return result;
        }
    }
}
