using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFPark.Core.Infrastructure;

namespace FFPark.Core.Configuration
{
    public class WebChatSettingsManager
    {
        /// <summary>
        /// 保存微信配置
        /// </summary>
        /// <param name="settings">cache settings</param>
        /// <param name="fileProvider">File provider</param>
        public static void SaveSettings(WebChatConfig settings, IFFParkFileProvider fileProvider = null)
        {
            Singleton<WebChatConfig>.Instance = settings ?? throw new ArgumentNullException(nameof(settings));

            fileProvider ??= CommonHelper.DefaultFileProvider;
            var filePath = fileProvider.MapPath(FFParkConfigurationDefaults.WebChatPath);

            //创建文件若不存在
            fileProvider.CreateFile(filePath);

            //向文件写入设置
            var text = JsonConvert.SerializeObject(Singleton<WebChatConfig>.Instance, Formatting.Indented);

            fileProvider.WriteAllText(filePath, text, Encoding.UTF8);
        }
        /// <summary>
        /// 加载微信配置
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="reloadSettings"></param>
        /// <param name="fileProvider"></param>
        /// <returns></returns>
        public static WebChatConfig LoadSettings(string filePath = null, bool reloadSettings = false, IFFParkFileProvider fileProvider = null)
        {
            if (!reloadSettings && Singleton<WebChatConfig>.Instance != null)
                return Singleton<WebChatConfig>.Instance;

            fileProvider ??= CommonHelper.DefaultFileProvider;
            filePath ??= fileProvider.MapPath(FFParkConfigurationDefaults.WebChatPath);

            //检查文件是否存在
            if (!fileProvider.FileExists(filePath))
            {
                if (!fileProvider.FileExists(filePath))
                    return new WebChatConfig();
            }
            var text = fileProvider.ReadAllText(filePath, Encoding.UTF8);
            if (string.IsNullOrEmpty(text))
                return new WebChatConfig();
            Singleton<WebChatConfig>.Instance = JsonConvert.DeserializeObject<WebChatConfig>(text);
            return Singleton<WebChatConfig>.Instance;
        }
    }
}
