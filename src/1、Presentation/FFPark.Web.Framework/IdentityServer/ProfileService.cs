using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FFPark.Core.Extensions;
using FFPark.Entity;
using FFPark.Services.Base;

namespace FFPark.Web.Framework.IdentityServer
{
    public class ProfileService<TUser> : IProfileService
    {
        private readonly IBaseUserServices _userServices;

        private readonly IUserRoleServcies _userRoleServices;
        public ProfileService(IBaseUserServices userServices, IUserRoleServcies userRoleServcies)
        {
            _userServices = userServices;
            _userRoleServices = userRoleServcies;
        }
        public async Task<List<Claim>> GetClaimsFromUser(BaseUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Subject,user.Id.ToString()),
                new Claim(JwtClaimTypes.PreferredUserName,user.UserName)
            };
            var roleNames = await _userRoleServices.GetRoleNames(user.Id);
            claims.Add(new Claim("userName", user.UserName));
            claims.Add(new Claim(JwtClaimTypes.Role, roleNames));
            return claims;
        }

        /// <summary>
        /// 获取用户Claims
        /// 用户请求userinfo endpoint时会触发该方法
        /// http://localhost:5001/connect/userinfo
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subjectId = context.Subject.Claims.FirstOrDefault(c => c.Type == "sub").Value;
            var user = await _userServices.FindByIdAsync(subjectId.ToInt());
            context.IssuedClaims = await GetClaimsFromUser(user);
        }
        /// <summary>
        /// 判断用户是否可用
        /// Identity Server会确定用户是否有效
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task IsActiveAsync(IsActiveContext context)
        {
            var subjectId = context.Subject.Claims.FirstOrDefault(c => c.Type == "sub").Value;
            var user = await _userServices.FindByIdAsync(subjectId.ToInt());
            context.IsActive = user != null; //该用户是否已经激活，可用，否则不能接受token
        }
        protected virtual async Task<BaseUser> FindUserAsync(string subjectId)
        {
            var user = await _userServices.FindByIdAsync(subjectId.ToInt());
            if (user == null)
            {
                //日志记录
            }
            return user;
        }
    }
}
