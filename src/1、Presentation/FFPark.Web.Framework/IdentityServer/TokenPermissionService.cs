using IdentityModel.Client;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FFPark.Web.Framework.IdentityServer
{
    public class TokenPermissionService : ITokenPermissionService
    {

        private string url = string.Empty;

        private IConfiguration _configuration;


        private string _url = "";
        DiscoveryCache _cache;

        public TokenPermissionService(IConfiguration configuration)
        {
            _configuration = configuration;
            _url = _configuration["AuthUrl"].ToString();
            _cache = new DiscoveryCache(_url);
        }
        static HttpClient _tokenClient = new HttpClient();

        public Task<string> CreateRefreshTokenAsync(ClaimsPrincipal subject, Token accessToken, Client client)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 请求token
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public async Task<TokenResponse> RequestTokenAsync(string username, string password)
        {
            var disco = await _cache.GetAsync();
            disco.Policy = new DiscoveryPolicy() { RequireHttps = true };
            var response = await _tokenClient.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "test",
                ClientSecret = "test",
                UserName = username,
                Password = password,
                Scope = "api1 offline_access",
                GrantType = GrantType.ResourceOwnerPassword
            });
            _tokenClient.SetBearerToken(response.AccessToken);

            if (response.IsError) throw new Exception(response.Error);
            return response;
        }


        public async Task<TokenResponse> RequestTokenByRefreshToken(string refreshToken)
        {

            var disco = await _cache.GetAsync();
            disco.Policy = new DiscoveryPolicy() { RequireHttps = false };
            var tokenResult = await _tokenClient.RequestRefreshTokenAsync(new RefreshTokenRequest()
            {
                Address = disco.TokenEndpoint,
                ClientId = "test",
                ClientSecret = "test",
                Scope = "api1 offline_access",
                GrantType = GrantType.ResourceOwnerPassword,
                RefreshToken = refreshToken,
            });
            return tokenResult;
        }


        /// <summary>
        /// 注销token
        /// </summary>
        /// <returns></returns>
        public async Task<TokenRevocationResponse> RevokeToken(string token)  
        {
            var disco = await _cache.GetAsync();
            disco.Policy = new DiscoveryPolicy() { RequireHttps = false };
            var tokenResult = await _tokenClient.RevokeTokenAsync(new TokenRevocationRequest()
            {
                Address = disco.RevocationEndpoint,
                ClientId = "test",
                ClientSecret = "test",
                Token = token
            });
            return tokenResult;
        }
        public Task<string> UpdateRefreshTokenAsync(string handle, RefreshToken refreshToken, Client client)
        {
            return null;
        }
        /// <summary>
        /// 验证刷新token
        /// </summary>
        /// <param name="token"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        public Task<TokenValidationResult> ValidateRefreshTokenAsync(string token, Client client)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 验证token 是否过期
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>

        public Task<bool> ValidaTokenExpireIn(string token)
        {
            throw new NotImplementedException();
        }
    }
}
