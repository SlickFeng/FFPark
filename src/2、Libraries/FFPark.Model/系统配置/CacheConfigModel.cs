using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFPark.Model
{
    public class CacheConfigModel : BaseFFParkModel
    {
        /// <summary>
        /// 默认缓存时间，单位分钟
        /// </summary>
        [Required(ErrorMessage = "默认缓存时间不可为空-单位分钟")]
        public int DefaultCacheTime { get; set; }

        [Required(ErrorMessage = "ShortTermCacheTime不可为空-单位分钟")]
        public int ShortTermCacheTime { get; set; } = 3;

        [Required(ErrorMessage = "BundledFilesCacheTime不可为空-单位分钟")]
        public int BundledFilesCacheTime { get; set; } = 120;
    }
}
