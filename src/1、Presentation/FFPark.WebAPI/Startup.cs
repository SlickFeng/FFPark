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
                string description = "�汾��v1.0.0.1";
                OpenApiLicense license = new OpenApiLicense()
                {
                    Name = "��˾�ڲ����",
                    Url = new Uri("http://www.shouzubei.com/api/licence"),

                };
                OpenApiContact contact = new OpenApiContact()
                {
                    Email = "wolferfeng@outlook.com",
                    Name = "����",
                    Url = new Uri("http://www.shouzubei.com/")
                };

                c.SwaggerDoc("v1",
                 new OpenApiInfo
                 {
                     Title = "���ͣ����API",
                     Version = version,
                     Description = description,
                     Contact = contact,
                     License = license
                 });
                c.SwaggerDoc("auth",
                    new OpenApiInfo
                    {
                        Title = "��¼ģ��",
                        Version = version,
                        Description = description,
                        Contact = contact,
                        License = license
                    });

                c.SwaggerDoc("pc",
                    new OpenApiInfo
                    {
                        Title = "PCģ��",
                        Version = version,
                        Description = description,
                        Contact = contact,
                        License = license
                    });

                c.SwaggerDoc("car", new OpenApiInfo
                {
                    Title = "С������ģ��",
                    Version = version,
                    Description = description,
                    Contact = contact,
                    License = license
                });

                c.SwaggerDoc("carmanager", new OpenApiInfo
                {
                    Title = "С���򳵳�",
                    Version = version,
                    Description = description,
                    Contact = contact,
                    License = license
                });



                c.OrderActionsBy(o => o.RelativePath);
                //����Ҫչʾ�Ľӿ�
                c.DocInclusionPredicate((docName, apiDes) =>
                {
                    if (!apiDes.TryGetMethodInfo(out MethodInfo method))
                        return false;
                    /*ʹ��ApiExplorerSettingsAttribute�����GroupName�������Ա�ʶ
                     * DeclaringTypeֻ�ܻ�ȡcontroller�ϵ�����
                     * ��������������action������Ϊ��
                     * 
                    */
                    var version = method.DeclaringType.GetCustomAttributes(true).OfType<ApiExplorerSettingsAttribute>().Select(m => m.GroupName);
                    if (docName == "v1" && !version.Any())
                        return true;
                    //�����ȡaction������
                    var actionVersion = method.GetCustomAttributes(true).OfType<ApiExplorerSettingsAttribute>().Select(m => m.GroupName);
                    if (actionVersion.Any())
                        return actionVersion.Any(v => v == docName);
                    return version.Any(v => v == docName);
                });
                //c.OperationFilter<WebUserAPI.Common.AssignOperationVendorExtensions>();
                //����SjiggJSON��UI��ע��·��.
                // Ϊ Swagger JSON and UI����xml�ĵ�ע��·��
                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);
                var xmlPath = Path.Combine(basePath, "FFPark.WebAPI.xml");
                var xmlmodelPath = Path.Combine(basePath, "FFPark.Model.xml");//���modelע��

                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                c.IncludeXmlComments(xmlmodelPath);
                c.IncludeXmlComments(xmlPath, true);//controllerע��;��������,�������ĻḲ��ǰ���

                //Bearer ��scheme����
                var securityScheme = new OpenApiSecurityScheme()
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    //���������ͷ��
                    In = ParameterLocation.Header,
                    //ʹ��Authorizeͷ��
                    Type = SecuritySchemeType.Http,
                    //����Ϊ�� bearer��ͷ
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                };
                //�����з�������Ϊ����bearerͷ����Ϣ
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
                //ע�ᵽswagger��
                c.AddSecurityDefinition("bearerAuth", securityScheme);
                c.AddSecurityRequirement(securityRequirement);

            });
        }
        /// <summary>
        /// ע��
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
                //����SwaggerUI��ʽ
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "����ģ��");
                    c.SwaggerEndpoint("/swagger/auth/swagger.json", "��¼ģ��");
                    c.SwaggerEndpoint("/swagger/pc/swagger.json", "PCģ��");
                    c.SwaggerEndpoint("/swagger/car/swagger.json", "С������ģ��");
                    c.SwaggerEndpoint("/swagger/carmanager/swagger.json", "С���򳵳�ģ��");
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

            //// �ȿ�����֤
            app.UseAuthentication();
            //// Ȼ������Ȩ�м��
            app.UseAuthorization();

            app.UseIdentityServer();

            app.UseHangfireServer();      //����hangfire����

            app.UseHangfireDashboard();   //ʹ��hangfire���

            //��ǩ
            // app.UseGlobalSign();

            //http �쳣���� ��������� �޷�����401 ,403 ���� 401 403 �������ط���IdentityServer ��
            app.UseFFParkExceptionHandler();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
