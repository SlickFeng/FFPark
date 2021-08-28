using IdentityModel.Client;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FFPark.Web.Framework.IdentityServer
{
    public interface ITokenPermissionService
    {
        /// <summary>
        /// 刷新token接口
        /// </summary>

        #region  刷新token
        /// <summary>
        /// Validates a refresh token.
        /// </summary>
        Task<TokenValidationResult> ValidateRefreshTokenAsync(string token, Client client);

        /// <summary>
        /// Creates the refresh token.
        /// </summary>
        Task<string> CreateRefreshTokenAsync(ClaimsPrincipal subject, Token accessToken, Client client);

        /// <summary>
        /// Updates the refresh token.
        /// </summary>
        Task<string> UpdateRefreshTokenAsync(string handle, RefreshToken refreshToken, Client client);

        #endregion

        #region   获取token
        Task<TokenResponse> RequestTokenAsync(string username, string password);
        /// <summary>
        /// 验证token 是否过期
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<bool> ValidaTokenExpireIn(string token);



        Task<TokenResponse> RequestTokenByRefreshToken(string refreshToken);
        #endregion

        #region   注销Token
        /// <summary>
        /// 注销token
        /// </summary>
        /// <returns></returns>
        Task<TokenRevocationResponse> RevokeToken(string token);
        #endregion

    }
}
