using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FFPark.Core.Http
{
    public class FFParkClient
    {
        private readonly HttpClient _client;

        public FFParkClient(HttpClient client)
        {
            _client = client;
        }
        /// <summary>
        /// 通用post 请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<T> PostAsync<T>(string url, object data) where T : class, new()
        {

            string content = JsonConvert.SerializeObject(data);
            var buffer = Encoding.UTF8.GetBytes(content);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await _client.PostAsync(url, byteContent).ConfigureAwait(false);
            string result = await response.Content.ReadAsStringAsync();
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return new T();
            }
            return JsonConvert.DeserializeObject<T>(result);
        }
        /// <summary>
        /// 通用get 请求
        /// </summary>
        /// <param name="requestUri">请求地址</param>
        /// <param name="data">请求参数,会自动拼接成a=1&b=2</param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(string requestUri, object data = null)
        {
            string requestUrl = string.Empty;
            if (data == null)
            {
                requestUrl = requestUri;
            }
            else
            {
                requestUrl = $"{requestUri}?{GetQueryString(data)}";
            }
            var response = await _client.GetAsync(requestUrl);
            string result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(result);
        }


        /// <summary>
        /// 微信通用get 请求
        /// </summary>
        /// <param name="requestUri">请求地址</param>
        /// <param name="data">请求参数,会自动拼接成a=1&b=2</param>
        /// <returns></returns>
        public async Task<T> GetWebChatAsync<T>(string requestUri, object data = null)
        {

            var response = await _client.GetAsync(requestUri).ConfigureAwait(false);
            string result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(result);
        }



        /// <summary>
        /// 微信post 专用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="accessToken"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<T> PostWebChatAsync<T>(string url, string accessToken, object data) where T : class, new()
        {

            url += "?access_token=" + accessToken;
            string content = JsonConvert.SerializeObject(data);

            var buffer = Encoding.UTF8.GetBytes(content);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await _client.PostAsync(url, byteContent).ConfigureAwait(false);
            string result = await response.Content.ReadAsStringAsync();

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return new T();
            }
            return JsonConvert.DeserializeObject<T>(result);
        }


        private static string GetQueryString(object obj)
        {
            if (obj == null)
            {
                return "";
            }
            var properties = from p in obj.GetType().GetProperties()
                             where p.GetValue(obj, null) != null
                             select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(obj, null).ToString());

            return String.Join("&", properties.ToArray());
        }

    }
}
