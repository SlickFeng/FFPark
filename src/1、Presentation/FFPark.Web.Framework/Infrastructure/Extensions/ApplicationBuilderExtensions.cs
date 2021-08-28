using Microsoft.AspNetCore.Builder;
using FFPark.Services.Common;
using FFPark.Web.Framework.IdentityServer;

namespace FFPark.Web.Framework.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// 异常捕获中间件-前台显示
        /// </summary>
        /// <param name="application"></param>
        public static void UseFFParkExceptionHandler(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<ErrorHandlingMiddleware>();
        }

        /// <summary>
        /// 验签中间件
        /// </summary>
        /// <param name="builder"></param>
        public static void UseGlobalSign(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<SignGlobalMiddleware>();
        }

        /// <summary>
        /// token 验证
        /// </summary>
        /// <param name="builder"></param>
        public static void UseIdentityServer4Auth(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<IdentityServer4Middleware>();
        }
    }
}

