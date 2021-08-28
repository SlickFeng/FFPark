using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFPark.Core.Infrastructure;

namespace FFPark.Core.Configuration
{
    public class CacheSettingsManager
    {
        /// <summary>
        /// Save data cache to the file
        /// </summary>
        /// <param name="settings">cache settings</param>
        /// <param name="fileProvider">File provider</param>
        public static void SaveSettings(CacheConfig settings, IFFParkFileProvider fileProvider = null)
        {
            Singleton<CacheConfig>.Instance = settings ?? throw new ArgumentNullException(nameof(settings));

            fileProvider ??= CommonHelper.DefaultFileProvider;
            var filePath = fileProvider.MapPath(FFParkConfigurationDefaults.CachePath);

            //创建文件若不存在
            fileProvider.CreateFile(filePath);

            //向文件写入设置
            var text = JsonConvert.SerializeObject(Singleton<CacheConfig>.Instance, Formatting.Indented);

            fileProvider.WriteAllText(filePath, text, Encoding.UTF8);
        }
        public static CacheConfig LoadSettings(string filePath = null, bool reloadSettings = false, IFFParkFileProvider fileProvider = null)
        {
            if (!reloadSettings && Singleton<CacheConfig>.Instance != null)
                return Singleton<CacheConfig>.Instance;

            fileProvider ??= CommonHelper.DefaultFileProvider;
            filePath ??= fileProvider.MapPath(FFParkConfigurationDefaults.CachePath);

            //检查文件是否存在
            if (!fileProvider.FileExists(filePath))
            {
                if (!fileProvider.FileExists(filePath))
                    return new CacheConfig();
            }
            var text = fileProvider.ReadAllText(filePath, Encoding.UTF8);
            if (string.IsNullOrEmpty(text))
                return new CacheConfig();
            Singleton<CacheConfig>.Instance = JsonConvert.DeserializeObject<CacheConfig>(text);

            return Singleton<CacheConfig>.Instance;
        }
    }
}
