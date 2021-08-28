using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFPark.Model
{
    public class WebChatModel : BaseFFParkModel
    {
        [Required(ErrorMessage = "AppId不可为空")]
        public string AppId { get; set; }

        [Required(ErrorMessage = "AppSecret不可为空")]
        public string AppSecret { get; set; }
        /// <summary>
        /// 商户号
        /// </summary>
        [Required(ErrorMessage = "商户号不可为空")]
        public string MerchId { get; set; }


        [Required(ErrorMessage = "Token不可为空")]
        public string Token { get; set; }


        [Required(ErrorMessage = "EncodingAESKey不可为空")]
        public string EncodingAESKey { get; set; }
    }
}
