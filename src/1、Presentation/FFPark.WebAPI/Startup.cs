using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using FFPark.Core.Configuration;
using FFPark.Core.Infrastructure;
using FFPark.Web.Framework.Infrastructure.Extensions;
using Hangfire.AspNetCore;
using Hangfire;

namespace FFPark.WebAPI
{
    public class Startup
    {

        private readonly IConfiguration _configuration;

        private readonly IWebHostEnvironment _webHostEnvironment;


        private AppSettings _appSettings;

        private IEngine _engine;

        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            (_engine, _appSettings) = services.ConfigureApplicationServices(_configuration, _webHostEnvironment);
            services.AddSwaggerGen(c =>
            {
                string version = "v1.0.0.1";
                string description = "版本：v1.0.0.1";
                OpenApiLicense license = new OpenApiLicense()
                {
                    Name = "公司内部许可",
                    Url = new Uri("http://www.shouzubei.com/api/licence"),

                };
                OpenApiContact contact = new OpenApiContact()
                {
                    Email = "wolferfeng@outlook.com",
                    Name = "晓峰",
                    Url = new Uri("http://www.shouzubei.com/")
                };

                c.SwaggerDoc("v1",
                 new OpenApiInfo
                 {
                     Title = "风风停车场API",
                     Version = version,
                     Description = description,
                     Contact = contact,
                     License = license
                 });
                c.SwaggerDoc("auth",
                    new OpenApiInfo
                    {
                        Title = "登录模块",
                        Version = version,
                        Description = description,
                        Contact = contact,
                        License = license
                    });

                c.SwaggerDoc("pc",
                    new OpenApiInfo
                    {
                        Title = "PC模块",
                        Version = version,
                        Description = description,
                        Contact = contact,
                        License = license
                    });

                c.SwaggerDoc("car", new OpenApiInfo
                {
                    Title = "小程序车主模块",
                    Version = version,
                    Description = description,
                    Contact = contact,
                    License = license
                });

                c.SwaggerDoc("carmanager", new OpenApiInfo
                {
                    Title = "小程序车场",
                    Version = version,
                    Description = description,
                    Contact = contact,
                    License = license
                });



                c.OrderActionsBy(o => o.RelativePath);
                //设置要展示的接口
                c.DocInclusionPredicate((docName, apiDes) =>
                {
                    if (!apiDes.TryGetMethodInfo(out MethodInfo method))
                        return false;
                    /*使用ApiExplorerSettingsAttribute里面的GroupName进行特性标识
                     * DeclaringType只能获取controller上的特性
                     * 我们这里是想以action的特性为主
                     * 
                    */
                    var version = method.DeclaringType.GetCustomAttributes(true).OfType<ApiExplorerSettingsAttribute>().Select(m => m.GroupName);
                    if (docName == "v1" && !version.Any())
                        return true;
                    //这里获取action的特性
                    var actionVersion = method.GetCustomAttributes(true).OfType<ApiExplorerSettingsAttribute>().Select(m => m.GroupName);
                    if (actionVersion.Any())
                        return actionVersion.Any(v => v == docName);
                    return version.Any(v => v == docName);
                });
                //c.OperationFilter<WebUserAPI.Common.AssignOperationVendorExtensions>();
                //设置SjiggJSON和UI的注释路径.
                // 为 Swagger JSON and UI设置xml文档注释路径
                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);
                var xmlPath = Path.Combine(basePath, "FFPark.WebAPI.xml");
                var xmlmodelPath = Path.Combine(basePath, "FFPark.Model.xml");//添加model注释

                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                c.IncludeXmlComments(xmlmodelPath);
                c.IncludeXmlComments(xmlPath, true);//controller注释;必须放最后,否则后面的会覆盖前面的

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
                c.AddSecurityDefinition("bearerAuth", securityScheme);
                c.AddSecurityRequirement(securityRequirement);

            });
        }
        /// <summary>
        /// 注入
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            _engine.RegisterDependencies(builder, _appSettings);
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();
                //启用SwaggerUI样式
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "公共模块");
                    c.SwaggerEndpoint("/swagger/auth/swagger.json", "登录模块");
                    c.SwaggerEndpoint("/swagger/pc/swagger.json", "PC模块");
                    c.SwaggerEndpoint("/swagger/car/swagger.json", "小程序车主模块");
                    c.SwaggerEndpoint("/swagger/carmanager/swagger.json", "小程序车场模块");
                    c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
                });
            }
            app.UseCors("FFPark");

            app.UseHttpsRedirection();

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            //   app.UseFFParkExceptionHandler();
            app.UseMiniProfiler();

            //// 先开启认证
            app.UseAuthentication();
            //// 然后是授权中间件
            app.UseAuthorization();

            app.UseIdentityServer();

            app.UseHangfireServer();      //启动hangfire服务

            app.UseHangfireDashboard();   //使用hangfire面板

            //验签
            // app.UseGlobalSign();

            //http 异常处理 放在最后面 无法拦截401 ,403 错误， 401 403 错误拦截放在IdentityServer 中
            app.UseFFParkExceptionHandler();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
