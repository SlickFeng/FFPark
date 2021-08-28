using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FFPark.WebAPI.Models
{
    public class AuthTokenModel
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public int ExpiresIn { get; set; }
    }
}
