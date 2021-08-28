using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Linq;
using FFPark.Core;
using FFPark.Core.Infrastructure;
using FreeSql;
using AspNetCoreRateLimit;
using FFPark.Core.Configuration;
using System.Net;
using FFPark.Web.Framework.Infrastructure.IdentityServer;
using FFPark.Web.Framework.Attribute;
using Newtonsoft.Json;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using Swashbuckle.AspNetCore.SwaggerGen;
using Hangfire;
using Hangfire.SqlServer;
using System.Data;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace FFPark.Web.Framework.Infrastructure.Extensions
{
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// 数据库FreeSql 配置
        /// </summary>
        /// <param name="service"></param>
        public static void UseFreeSql(this IServiceCollection service)
        {
            //加载配置类

            DataConfig dataSettings = DataSettingsManager.LoadSettings();

            //注入FreeSql
            service.AddSingleton<IFreeSql>(f =>
            {
                var log = f.GetRequiredService<ILogger<IFreeSql>>();
                var freeBuilder = new FreeSqlBuilder()
                    .UseAutoSyncStructure(true)
                    .UseConnectionString((FreeSql.DataType)dataSettings.DataType, dataSettings.MasterConnetion)
                    .UseLazyLoading(true)
                    .UseMonitorCommand(
                    executing =>
                    {
                        //执行中打印日志
                        log.LogInformation(executing.CommandText);
                    });
                if (dataSettings.SlaveConnections?.Count > 0)//判断是否存在从库
                {
                    freeBuilder.UseSlave(dataSettings.SlaveConnections.Select(x => x.ConnectionString).ToArray());
                }
                service.AddFreeRepository();
                var freesql = freeBuilder.Build();
                return freesql;
            });
        }
        public static (IEngine, AppSettings) ConfigureApplicationServices(this IServiceCollection services,
                       IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            CommonHelper.DefaultFileProvider = new FFParkFileProvider(webHostEnvironment);
            //配置跨域处理
            services.AddCors(c =>
            {
                //允许任意跨域请求
                c.AddPolicy("FFPark",
                    policy =>
                    {
                        policy
                        .SetIsOriginAllowed((host) => true)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                    });
            });
            //注入HttpContext 
            services.AddHttpContextAccessor();


            //freesql 配置
            services.UseFreeSql();

            //IdentityServer4 配置
            services.UseIdentityServer4();
            //加入拦截器
            services.AddControllers(options =>
            {
                //注册全全局模型验证过滤器
                options.Filters.Add(new ModelActionFilter());

            }).ConfigureApiBehaviorOptions(options =>
               {
                   //抑制系统自带模型验证
                   options.SuppressModelStateInvalidFilter = true;
               }).AddControllersAsServices().AddNewtonsoftJson(options =>
                          {
                              options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                              options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
                              options.SerializerSettings.DateFormatString = "yyyy-MM-dd hh:mm:ss";
                          }).AddXmlSerializerFormatters();

            services.AddHttpContextAccessor();

            //暂时配置
            //  services.UseSwagger();

            //性能监控插件
            services.MiniProfiler();




            //配置文件配置-将采用中间件的形式-此处屏蔽  --勿删除
            var appSettings = new AppSettings();
            configuration.Bind(appSettings);
            services.AddSingleton(appSettings);

            services.UseHangFire();

            services.AddRouting(options => options.LowercaseUrls = true);
            var engine = EngineContext.Create();

            engine.ConfigureServices(services, configuration);

            return (engine, appSettings);

        }

        public static void UseSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                new OpenApiInfo
                {
                    Title = "收租呗API",

                    Version = "v1.0.0.1",
                    Description = "该程序采用.Net 5 开发",
                    Contact = new OpenApiContact()
                    {
                        Email = "920260851@qq.com",
                        Name = "井晓峰",
                        Url = new Uri("http://www.shouzubei.com/")
                    },
                    License = new OpenApiLicense()
                    {
                        Name = "公司内部许可",
                        Url = new Uri("http://www.shouzubei.com/api/licence")
                    }
                });


                options.SwaggerDoc("Auth", new OpenApiInfo { Title = "授权", Version = "Auth", Description = "授权 接口" });
                options.SwaggerDoc("PC", new OpenApiInfo { Title = "PC 后台", Version = "PC", Description = "PC 后台API接口" });
                options.SwaggerDoc("SmallProgram", new OpenApiInfo { Title = "小程序", Version = "SmallProgram", Description = "小程序API 接口" });

                options.OrderActionsBy(o => o.RelativePath);
        //设置要展示的接口

        options.DocInclusionPredicate((docName, apiDes) =>
{
                    if (!apiDes.TryGetMethodInfo(out MethodInfo method))
                        return false;
            /*使用ApiExplorerSettingsAttribute里面的GroupName进行特性标识
             * DeclaringType只能获取controller上的特性
             * 我们这里是想以action的特性为主
             * */
                    var version = method.DeclaringType.GetCustomAttributes(true).OfType<ApiExplorerSettingsAttribute>().Select(m => m.GroupName);
                    if (docName == "v1" && !version.Any())
                        return true;
            //这里获取action的特性
            var actionVersion = method.GetCustomAttributes(true).OfType<ApiExplorerSettingsAttribute>().Select(m => m.GroupName);
                    if (actionVersion.Any())
                        return actionVersion.Any(v => v == docName);
                    return version.Any(v => v == docName);
                });

                var basePath = Path.GetDirectoryName(typeof(ServiceCollectionExtensions).Assembly.Location);
                var xmlPath = Path.Combine(basePath, "FFPark.WebAPI.xml");
                var xmlmodelPath = Path.Combine(basePath, "FFPark.Model.xml");//添加model注释

                options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                options.IncludeXmlComments(xmlmodelPath);
                options.IncludeXmlComments(xmlPath, true);//controller注释;必须放最后,否则后面的会覆盖前面的

        //Bearer 的scheme定义
        var securityScheme = new OpenApiSecurityScheme()
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
            //参数添加在头部
            In = ParameterLocation.Header,
            //使用Authorize头部
            Type = SecuritySchemeType.Http,
            //内容为以 bearer开头
            Scheme = "bearer",
                    BearerFormat = "JWT"
                };


                options.DocInclusionPredicate((docName, description) => true);


        //把所有方法配置为增加bearer头部信息
        var securityRequirement = new OpenApiSecurityRequirement
                    {
                        {
                                new OpenApiSecurityScheme
                                {
                                    Reference = new OpenApiReference
                                    {
                                        Type = ReferenceType.SecurityScheme,
                                        Id = "bearerAuth"
                                    }
                                },
                                new string[] {}
                        }
                    };

        //注册到swagger中
        options.AddSecurityDefinition("bearerAuth", securityScheme);
                options.AddSecurityRequirement(securityRequirement);
            });
        }
        public static void UseRateLimit(this IServiceCollection services, IConfiguration Configuration)
        {
            // needed to store rate limit counters and ip rules
            services.AddMemoryCache();

            // configure ip rate limiting middleware
            services.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimiting"));
            services.Configure<IpRateLimitPolicies>(Configuration.GetSection("IpRateLimitPolicies"));
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            // configure client rate limiting middleware
            services.Configure<ClientRateLimitOptions>(Configuration.GetSection("ClientRateLimiting"));
            services.Configure<ClientRateLimitPolicies>(Configuration.GetSection("ClientRateLimitPolicies"));
            services.AddSingleton<IClientPolicyStore, MemoryCacheClientPolicyStore>();
            //services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddMvc((options) =>
            {
                options.EnableEndpointRouting = false;
            }).AddNewtonsoftJson();
            // configure the resolvers
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        }

        /// <summary>
        /// 性能监控插件
        /// </summary>
        /// <param name="services"></param>
        public static void MiniProfiler(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddMiniProfiler();
        }


        public static void UseHangFire(this IServiceCollection services)
        {
            DataConfig dataSettings = DataSettingsManager.LoadSettings();
            services.AddHangfire(configuration => configuration.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                               .UseSimpleAssemblyNameTypeSerializer()
                               .UseRecommendedSerializerSettings()
                               .UseSqlServerStorage(dataSettings.MasterConnetion, new SqlServerStorageOptions
                               {
                                   CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                                   SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                                   QueuePollInterval = TimeSpan.Zero,
                                   UseRecommendedIsolationLevel = true,
                                   UsePageLocksOnDequeue = true,
                                   DisableGlobalLocks = true
                               }));
            services.AddHangfireServer();
        }
    }
}
