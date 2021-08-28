using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFPark.Core;
using FFPark.Core.Configuration;
using FFPark.Core.Http;
using FFPark.Core.Infrastructure;
using FFPark.Core.Infrastructure.DependencyManagement;
using FFPark.Core.Result;
using FFPark.Core.WebChat;
using FFPark.Data;
using FFPark.Entity.Security;
using FFPark.Services.Base;
using FFPark.Services.Park;
using FFPark.Services.Pay;
using FFPark.Services.Security;
using FFPark.Services.WebChat;
using FFPark.Web.Framework.IdentityServer;
using FFPark.Services;

namespace FFPark.Web.Framework.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        /// <summary>
        /// Register services and interfaces
        /// </summary>
        /// <param name="builder">Container builder</param>
        /// <param name="typeFinder">Type finder</param>
        /// <param name="appSettings">App settings</param>
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder, AppSettings appSettings)
        {
            //json  message
            builder.RegisterType<ApiResultBase>().As<IApiResult>().InstancePerLifetimeScope();

            //file provider
            builder.RegisterType<FFParkFileProvider>().As<IFFParkFileProvider>().InstancePerLifetimeScope();

            //web helper
            builder.RegisterType<WebHelper>().As<IWebHelper>().InstancePerLifetimeScope();

            //repositories
            builder.RegisterGeneric(typeof(EntityRepository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();

            //plugins 注入  预留 不要删除
            //redis connection wrapper  
            // if (appSettings.RedisConfig.Enabled)
            // {
            //builder.RegisterType<RedisConnectionWrapper>()
            //    .As<ILocker>()
            //    .As<IRedisConnectionWrapper>()
            //    .SingleInstance();
            // }
            //static cache manager
            // if (appSettings.RedisConfig.Enabled && appSettings.RedisConfig.UseCaching)
            // {
            //  builder.RegisterType<RedisCacheManager>().As<IStaticCacheManager>().InstancePerLifetimeScope();
            // }
            // else
            // {
            //builder.RegisterType<MemoryCacheManager>()
            //    .As<ILocker>()
            //    .As<IStaticCacheManager>()
            //    .SingleInstance();
            // }

            //work context  工作上下文
            // builder.RegisterType<WebWorkContext>().As<IWorkContext>().InstancePerLifetimeScope();

            builder.RegisterType<ApiResultBase>().As<IApiResult>().InstancePerLifetimeScope();


            ////token 验证,生成相关
            builder.RegisterType<TokenPermissionService>().As<ITokenPermissionService>().InstancePerLifetimeScope();

            //services
            builder.RegisterType<BaseUserServices>().As<IBaseUserServices>().InstancePerLifetimeScope();

            builder.RegisterType<UserRoleServcies>().As<IUserRoleServcies>().InstancePerLifetimeScope();

            builder.RegisterType<BaseRoleServices>().As<IBaseRoleServices>().InstancePerLifetimeScope();


            builder.RegisterType<PayChannelServices>().As<IPayChannelServices>().InstancePerLifetimeScope();

            builder.RegisterType<WebChatConfigServices>().As<IWebChatConfigServices>().InstancePerLifetimeScope();

            builder.RegisterType<WebChatNoticeServcies>().As<IWebChatNoticeServices>().InstancePerLifetimeScope();



            //http
            builder.RegisterType<FFParkClient>().InstancePerLifetimeScope();

            builder.RegisterType<WebChatClient>().InstancePerLifetimeScope();

            builder.RegisterType<SecuritySettings>().InstancePerLifetimeScope();
            //加密
            builder.RegisterType<EncryptionService>().As<IEncryptionService>().InstancePerLifetimeScope();


            builder.RegisterType<SubscriptionMessageRequestHandler>().As<ISubscriptionMessageRequestHandler>().InstancePerLifetimeScope();


            builder.RegisterType<WebChatNoticeServcies>().As<IWebChatNoticeServices>().InstancePerLifetimeScope();


            builder.RegisterType<WebChatConfigServices>().As<IWebChatConfigServices>().InstancePerLifetimeScope();

            //通用广告
            builder.RegisterType<AdCurrencyPresentationServices>().As<IAdCurrencyPresentationServices>().InstancePerLifetimeScope();

            //车牌绑定
            builder.RegisterType<LicenseBindServcies>().As<ILicenseBindServices>().InstancePerLifetimeScope();

            //优惠券
            builder.RegisterType<CarSpecialDiscountServices>().As<ICarSpecialDiscountServices>().InstancePerLifetimeScope();

            builder.RegisterType<SpecialDiscountServices>().As<ISpecialDiscountServices>().InstancePerLifetimeScope();

            //广告位置
            builder.RegisterType<AdPositionServices>().As<IAdPositionServices>().InstancePerLifetimeScope();

            builder.RegisterType<BaseNavigationModuleServices>().As<IBaseNavigationModuleServices>().InstancePerLifetimeScope();

            //车场管理
            builder.RegisterType<BaseParkServices>().As<IBaseParkServices>().InstancePerLifetimeScope();

            builder.RegisterType<CodeFirstServices>().As<ICodeFirstServices>().InstancePerLifetimeScope();

            //快捷导航
            builder.RegisterType<FastNavigationServices>().As<IFastNavigationServices>().InstancePerLifetimeScope();

        }

        /// <summary>
        /// Gets order of this dependency registrar implementation
        /// </summary>
        public int Order => 0;
    }
}
