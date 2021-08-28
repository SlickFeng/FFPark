using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FFPark.Web.Framework.Infrastructure.IdentityServer
{
    public class TokenConfig
    {
        /// <summary>
        /// 定义资源范围
        /// </summary>
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource> { new ApiResource("api1", "我的第一个API") };
        }

        /// <summary>
        /// 定义访问的资源客户端
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Client> GetClients =>
                            new List<Client>
                            {
                                new Client()
                                {
                                    ClientId="test",
                                    ClientSecrets=new []{new Secret("test".Sha256())},
                                    AllowedGrantTypes=GrantTypes.ResourceOwnerPassword,
                                    AccessTokenLifetime=3600,
                                    AllowedScopes={  "api1" },
                                    AllowOfflineAccess=true

                                },
                            };
        public static IEnumerable<ApiScope> GetApiScopes =>
                        new List<ApiScope> { new ApiScope("api1") };
        /// <summary>
        /// 这个方法是来规范tooken生成的规则和方法的。一般不进行设置，直接采用默认的即可。
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                   new IdentityResources.OpenId(),
                   new IdentityResources.Profile()
            };
        }
    }
}
