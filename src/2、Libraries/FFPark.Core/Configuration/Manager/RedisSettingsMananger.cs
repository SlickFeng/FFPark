using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFPark.Core;
using FFPark.Core.Configuration;
using FFPark.Core.Infrastructure;

namespace FFPark.Core.Configuration
{
    public partial class RedisSettingsManager
    {
        /// <summary>
        /// Save Redis settings to the file
        /// </summary>
        /// <param name="settings">Redis settings</param>
        /// <param name="fileProvider">File provider</param>
        public static void SaveSettings(RedisConfig settings, IFFParkFileProvider fileProvider = null)
        {
            Singleton<RedisConfig>.Instance = settings ?? throw new ArgumentNullException(nameof(settings));

            fileProvider ??= CommonHelper.DefaultFileProvider;
            var filePath = fileProvider.MapPath(FFParkConfigurationDefaults.RedisPath);

            //创建文件若不存在
            fileProvider.CreateFile(filePath);

            //向文件写入设置
            var text = JsonConvert.SerializeObject(Singleton<RedisConfig>.Instance, Formatting.Indented);

            fileProvider.WriteAllText(filePath, text, Encoding.UTF8);
        }
        public static RedisConfig LoadSettings(string filePath = null, bool reloadSettings = false, IFFParkFileProvider fileProvider = null)
        {
            if (!reloadSettings && Singleton<RedisConfig>.Instance != null)
                return Singleton<RedisConfig>.Instance;

            fileProvider ??= CommonHelper.DefaultFileProvider;
            filePath ??= fileProvider.MapPath(FFParkConfigurationDefaults.RedisPath);

            //检查文件是否存在
            if (!fileProvider.FileExists(filePath))
            {
                if (!fileProvider.FileExists(filePath))
                    return new RedisConfig();
            }
            var text = fileProvider.ReadAllText(filePath, Encoding.UTF8);
            if (string.IsNullOrEmpty(text))
                return new RedisConfig();
            Singleton<RedisConfig>.Instance = JsonConvert.DeserializeObject<RedisConfig>(text);

            return Singleton<RedisConfig>.Instance;
        }

    }
}
