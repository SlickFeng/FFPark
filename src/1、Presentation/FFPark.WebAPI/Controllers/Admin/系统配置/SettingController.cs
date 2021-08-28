using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FFPark.Core.Configuration;
using FFPark.Core.Infrastructure;
using FFPark.Core.Result;
using FFPark.Model;
using FFPark.Web.Framework.Attributes;
using FFPark.Web.Framework.Controllers;
using FFPark.WebAPI.Infrastructure.Mapper.Extensions;
using FFPark.Services;

namespace FFPark.WebAPI.Controllers.Admin
{
    /// <summary>
    /// 系统设置
    /// </summary>
    [ApiController]
    [Route("api/sys/[action]")]
    [ApiAuthorize(Roles = "SystemAdmin")]
    [ApiExplorerSettings(GroupName = "pc")]
    public class SettingController : BaseAuthorizeController
    {
        #region 字段
        private readonly AppSettings _appSettings;
        private readonly IFFParkFileProvider _fileProvider;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly IApiResult _apiResult;

       
        #endregion

        public SettingController(
            IApiResult apiResult,
            AppSettings appSettings,
            IFFParkFileProvider fileProvider,
            IHostApplicationLifetime hostApplicationLifetime)
        {
            _apiResult = apiResult;
            _appSettings = appSettings;
            _fileProvider = fileProvider;
            _hostApplicationLifetime = hostApplicationLifetime;
          
        }

      
        [HttpPost]
        public IActionResult RedisWrite()
        {

            int id = WorkContext.UserId;
            RedisConfig redisConfig = new RedisConfig()
            {
                ConnectionString = "Redis 链接字符串",
                DatabaseId = 0,
                Enabled = false,
                IgnoreTimeoutException = false,
                StoreDataProtectionKeys = false,
                UseCaching = false
            };
            RedisSettingsManager.SaveSettings(redisConfig, _fileProvider);
            return new JsonResult(_apiResult.SetOK("写入成功,重启应用程序后才能生效"));
        }
        [HttpPost]
        public IActionResult RedisRead()
        {
            RedisConfig redisConfig = RedisSettingsManager.LoadSettings();
            return new JsonResult(_apiResult.SetOK("读取成功", redisConfig));
        }


        /// <summary>
        /// 写入内存缓存配置参数
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel> CacheWrite([FromBody] CacheConfigModel model)
        {

            CacheConfig entity = model.ToEntity<CacheConfig>();
            await Task.Run(() =>
            {
                CacheSettingsManager.SaveSettings(entity);
            });
            return SuccessMsg("操作成功");
        }
        /// <summary>
        /// 读取内存缓存配置参数
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        public async Task<MessageModel<CacheConfigModel>> CacheRead()
        {

            CacheConfig settings = new CacheConfig();
            await Task.Run(() =>
            {
                settings = CacheSettingsManager.LoadSettings();
            });

            CacheConfigModel model = settings.ToModel<CacheConfigModel>();
            return Success(model);
        }


        [HttpPost]
        public IActionResult DbWrite()
        {
            DataConfig config = new DataConfig();
            List<SlaveConnection> slaveConnections = new List<SlaveConnection>();
            for (int i = 0; i < 3; i++)
            {
                slaveConnections.Add(new SlaveConnection() { ConnectionString = "新链接" + i });
            }
            config.SlaveConnections = slaveConnections;
            config.DataType = FreeSql.DataType.SqlServer;
            config.MasterConnetion = "Data Source=183.56.204.226;Initial Catalog=CloudHIS;User ID=sa;Password=Hjyy2020";
            DataSettingsManager.SaveSettings(config, _fileProvider);
            return new JsonResult(_apiResult.SetOK("写入成功"));
        }

        [HttpPost]
        public IActionResult DbRead()
        {
            DataConfig settings = DataSettingsManager.LoadSettings();
            return new JsonResult(_apiResult.SetOK("读取成功", settings));
        }

        /// <summary>
        /// 系统重启,只要重新请求Url 网站即被激活了
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel> Restart()
        {
            await Task.Run(() =>
            {
                _hostApplicationLifetime.StopApplication();
            });
            return SuccessMsg("重启成功");
        }
        /// <summary>
        /// 读取微信相关配置信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<WebChatModel>> WebChatConfigRead()
        {
            WebChatConfig settings = new WebChatConfig();
            await Task.Run(() =>
             {
                 settings = WebChatSettingsManager.LoadSettings();
             });
            WebChatModel model = settings.ToModel<WebChatModel>();
            return Success(model);
        }

        /// <summary>
        /// 保存微信相关配置信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel> WebChatConfigWrite([FromBody] WebChatModel model)
        {
            WebChatConfig entity = model.ToEntity<WebChatConfig>();
            await Task.Run(() =>
            {
                WebChatSettingsManager.SaveSettings(entity);
            });
            return SuccessMsg("操作成功");
        }
    }
}
