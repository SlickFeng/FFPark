using Microsoft.Extensions.DependencyInjection;
using FFPark.Entity;
namespace FFPark.Web.Framework.IdentityServer.DependencyInjection
{
    public static class IdentityServerBuilderExtensions
    {
        public static IIdentityServerBuilder AddAspNetIdentity<TUser>(this IIdentityServerBuilder builder) where TUser : class
        {

            builder.AddResourceOwnerValidator<ResourceOwnerPasswordValidator<BaseUser>>();
            builder.AddProfileService<ProfileService<BaseUser>>();
            return builder;
        }
    }
}
