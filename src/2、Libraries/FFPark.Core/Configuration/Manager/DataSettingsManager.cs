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
    public partial class DataSettingsManager
    {
        /// <summary>
        /// Save data settings to the file
        /// </summary>
        /// <param name="settings">Data settings</param>
        /// <param name="fileProvider">File provider</param>
        public static void SaveSettings(DataConfig settings, IFFParkFileProvider fileProvider = null)
        {
            Singleton<DataConfig>.Instance = settings ?? throw new ArgumentNullException(nameof(settings));

            fileProvider ??= CommonHelper.DefaultFileProvider;
            var filePath = fileProvider.MapPath(FFParkConfigurationDefaults.FreeSqlFilePath);

            //创建文件若不存在
            fileProvider.CreateFile(filePath);

            //向文件写入设置
            var text = JsonConvert.SerializeObject(Singleton<DataConfig>.Instance, Formatting.Indented);
          
            fileProvider.WriteAllText(filePath, text, Encoding.UTF8);
        }


        public static DataConfig LoadSettings(string filePath = null, bool reloadSettings = false, IFFParkFileProvider fileProvider = null)
        {
            if (!reloadSettings && Singleton<DataConfig>.Instance != null)
                return Singleton<DataConfig>.Instance;

            fileProvider ??= CommonHelper.DefaultFileProvider;
            filePath ??= fileProvider.MapPath(FFParkConfigurationDefaults.FreeSqlFilePath);

            //检查文件是否存在
            if (!fileProvider.FileExists(filePath))
            {
                if (!fileProvider.FileExists(filePath))
                    return new DataConfig();
            }
            var text = fileProvider.ReadAllText(filePath, Encoding.UTF8);
            if (string.IsNullOrEmpty(text))
                return new DataConfig();
            Singleton<DataConfig>.Instance = JsonConvert.DeserializeObject<DataConfig>(text);

            return Singleton<DataConfig>.Instance;
        }

    }
}
