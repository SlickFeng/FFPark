using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FFPark.WebAPI.Models
{
    public class SignModel
    {
        [Required(ErrorMessage = "AppId未提供")]
        public string AppId { get; set; }

        [Required(ErrorMessage = "AppSecret未提供")]
        [MinLength(6, ErrorMessage = "AppSecret长度至少6位")]
        public string AppSecret { get; set; }

    }
}
