using IdentityServer4.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FFPark.Web.Framework.IdentityServer
{
   public class TokenTwzRequest
    {
        public static string ClientId = "ClientId";

        public static string ClientSecret = "6KGqzUx6nfZZp0a4NH2xenWSJQWAT8la";

        public static string Scope = "api";


        public static List<string> Scopes = new List<string>() { "api" };


        public static string GrantTypes = GrantType.ClientCredentials;


        public static string GrangTypePassword = GrantType.ResourceOwnerPassword;


        public static string ClientId_Password = "ClientId_Password";


        public static ClaimsPrincipal Validate_Token(string token)
        {
            var validationParameters =
                                     new TokenValidationParameters
                                     {
                                         ValidAudiences = new[] { TokenTwzRequest.ClientId_Password },
                                     };
            SecurityToken validatedToken;
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            var user = handler.ValidateToken(token, validationParameters, out validatedToken);
            return user;
        }
    }
}
