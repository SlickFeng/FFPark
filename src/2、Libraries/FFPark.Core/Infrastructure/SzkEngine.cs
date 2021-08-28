using Autofac;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FFPark.Core.Configuration;
using FFPark.Core.Infrastructure.DependencyManagement;
using FFPark.Core.Infrastructure.Mapper;
using FFPark.Core.Security;
namespace FFPark.Core.Infrastructure
{
    /// <summary>
    /// 收租快WebAPI 引擎
    /// </summary>
    public class FFParkEngine : IEngine
    {
        #region 字段

        private ITypeFinder _typeFinder;

        #endregion

        #region 公共

        /// <summary>
        /// Get IServiceProvider
        /// </summary>
        /// <returns>IServiceProvider</returns>
        protected IServiceProvider GetServiceProvider()
        {
            if (ServiceProvider == null)
                return null;
            var accessor = ServiceProvider?.GetService<IHttpContextAccessor>();
            var context = accessor?.HttpContext;
            return context?.RequestServices ?? ServiceProvider;
        }

        /// <summary>
        /// Run startup tasks
        /// </summary>
        /// <param name="typeFinder">Type finder</param>
        protected virtual void RunStartupTasks(ITypeFinder typeFinder)
        {

            var startupTasks = typeFinder.FindClassesOfType<IStartupTask>();

            var instances = startupTasks
                .Select(startupTask => (IStartupTask)Activator.CreateInstance(startupTask))
                .OrderBy(startupTask => startupTask.Order);


            foreach (var task in instances)
                task.ExecuteAsync().Wait();
        }

        /// <summary>
        /// Register dependencies
        /// </summary>
        /// <param name="containerBuilder">Container builder</param>
        /// <param name="appSettings">App settings</param>
        public virtual void RegisterDependencies(ContainerBuilder containerBuilder, AppSettings appSettings)
        {

            containerBuilder.RegisterInstance(this).As<IEngine>().SingleInstance();


            containerBuilder.RegisterInstance(_typeFinder).As<ITypeFinder>().SingleInstance();


            var dependencyRegistrars = _typeFinder.FindClassesOfType<IDependencyRegistrar>();


            var instances = dependencyRegistrars
                .Select(dependencyRegistrar => (IDependencyRegistrar)Activator.CreateInstance(dependencyRegistrar))
                .OrderBy(dependencyRegistrar => dependencyRegistrar.Order);

            //注册所有依赖
            foreach (var dependencyRegistrar in instances)
                dependencyRegistrar.Register(containerBuilder, _typeFinder, appSettings);
        }

        /// <summary>
        /// 注册配置 AutoMapper
        protected virtual void AddAutoMapper(IServiceCollection services, ITypeFinder typeFinder)
        {

            var mapperConfigurations = typeFinder.FindClassesOfType<IOrderedMapperProfile>();
            var instances = mapperConfigurations
                .Select(mapperConfiguration => (IOrderedMapperProfile)Activator.CreateInstance(mapperConfiguration))
                .OrderBy(mapperConfiguration => mapperConfiguration.Order);
            var config = new MapperConfiguration(cfg =>
            {
                foreach (var instance in instances)
                {
                    cfg.AddProfile(instance.GetType());
                }
            });
            //注册初始化
            AutoMapperConfiguration.Init(config);
        }

        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            //check for assembly already loaded
            var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName == args.Name);
            if (assembly != null)
                return assembly;

            //get assembly from TypeFinder
            var tf = Resolve<ITypeFinder>();
            if (tf == null)
                return null;
            assembly = tf.GetAssemblies().FirstOrDefault(a => a.FullName == args.Name);
            return assembly;
        }

        #endregion

        #region Methods


        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            //find startup configurations provided by other assemblies
            _typeFinder = new WebAppTypeFinder();
            var startupConfigurations = _typeFinder.FindClassesOfType<IFFParkStartup>();


            //create and sort instances of startup configurations
            var instances = startupConfigurations
                .Select(startup => (IFFParkStartup)Activator.CreateInstance(startup))
                .OrderBy(startup => startup.Order);

            //configure services
            foreach (var instance in instances)
                instance.ConfigureServices(services, configuration);
            AddAutoMapper(services, _typeFinder);

            RunStartupTasks(_typeFinder);


            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        /// <summary>
        /// HTTP request 管道
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public void ConfigureRequestPipeline(IApplicationBuilder application)
        {
            ServiceProvider = application.ApplicationServices;

            //find startup configurations provided by other assemblies
            var typeFinder = Resolve<ITypeFinder>();
            var startupConfigurations = typeFinder.FindClassesOfType<IFFParkStartup>();

            //create and sort instances of startup configurations
            var instances = startupConfigurations
                .Select(startup => (IFFParkStartup)Activator.CreateInstance(startup))
                .OrderBy(startup => startup.Order);





            //configure request pipeline
            foreach (var instance in instances)
                instance.Configure(application);

           

        }


        public T Resolve<T>() where T : class
        {
            return (T)Resolve(typeof(T));
        }


        public object Resolve(Type type)
        {
            var sp = GetServiceProvider();
            if (sp == null)
                return null;
            return sp.GetService(type);
        }


        public virtual IEnumerable<T> ResolveAll<T>()
        {
            return (IEnumerable<T>)GetServiceProvider().GetServices(typeof(T));
        }


        public virtual object ResolveUnregistered(Type type)
        {
            Exception innerException = null;
            foreach (var constructor in type.GetConstructors())
            {
                try
                {
                    //try to resolve constructor parameters
                    var parameters = constructor.GetParameters().Select(parameter =>
                    {
                        var service = Resolve(parameter.ParameterType);
                        if (service == null)
                            throw new FFParkException("Unknown dependency");
                        return service;
                    });

                    //all is ok, so create instance
                    return Activator.CreateInstance(type, parameters.ToArray());
                }
                catch (Exception ex)
                {
                    innerException = ex;
                }
            }

            throw new FFParkException("No constructor was found that had all the dependencies satisfied.", innerException);
        }

        #endregion

        #region 属性


        public virtual IServiceProvider ServiceProvider { get; protected set; }

        #endregion
    }
}
