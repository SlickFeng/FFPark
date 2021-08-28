using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFPark.Core.Configuration
{
    public partial class FFParkConfigurationDefaults
    {

        public static string FilePath => "FileSettings/";

        /// <summary>
        /// appsettings.json 配置
        /// </summary>
        public static string AppSettingsFilePath => FilePath + "appsettings.json";


        /// <summary>
        /// FreeSql 配置
        /// </summary>
        public static string FreeSqlFilePath => FilePath + "freeSql.json";



        public static string RabbitMqPath => FilePath + "rabbitMq.json";
        /// <summary>
        /// Serilog 配置文件
        /// </summary>

        public static string SerilogPath => FilePath + "serilog.json";


        /// <summary>
        /// mongodb 配置
        /// </summary>

        public static string MongodbPath => FilePath + "mongodb.json";


        /// <summary>
        /// 流量分析 配置
        /// </summary>
        public static string RequestAnalysis => FilePath + "RequestAnalysis.json";

        /// <summary>
        /// cache 配置
        /// </summary>

        public static string CachePath => FilePath + "cache.json";

        /// <summary>
        /// redis 缓存配置
        /// </summary>
        public static string RedisPath => FilePath + "redis.json";


        public static string WebChatPath => FilePath + "webChat.json";
    }
}
