using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using FFPark.Entity;
using FFPark.Web.Framework.IdentityServer;
using FFPark.Web.Framework.IdentityServer.DependencyInjection;
namespace FFPark.Web.Framework.Infrastructure.IdentityServer
{
    public static partial class ICollectionServiceExtensions
    {
        public static void UseIdentityServer4(this IServiceCollection services)
        {
            services.AddIdentityServer()//注册服务
                 .AddDeveloperSigningCredential()
                 .AddInMemoryApiResources(TokenConfig.GetApiResources())//配置类定义的授权范围
                 .AddInMemoryClients(TokenConfig.GetClients)//配置类定义的授权客户端
                 .AddInMemoryApiScopes(TokenConfig.GetApiScopes)
                 .AddAspNetIdentity<BaseUser>();
        }
    }
}
