using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFPark.Model
{
    public class AdCurrencyPresentationModel : BaseFFParkModel
    {
        /// <summary>
        /// 广告展示位置
        /// </summary>
        [Required(ErrorMessage = "广告展示位置不可为空")]
        public string LayerName { get; set; }
        /// <summary>
        /// 广告图片地址
        /// </summary>
        [Required(ErrorMessage = "广告图片不可为空")]

        public string AdImageUrl { get; set; }
        /// <summary>
        /// 广告名称
        /// </summary>
        /// </summary>
        [Required(ErrorMessage = "广告名称")]
        public string AdName { get; set; }
        /// <summary>
        /// 跳转Http 地址
        /// </summary>
        public string TargetHttpUrl { get; set; }
        /// <summary>
        /// 广告位跳转小程序地址
        /// </summary>
        public string SmallProgramTargetUrl { get; set; }


        public string Remark { get; set; }
        /// <summary>
        /// 排序字段，越大越靠前
        /// </summary>
        [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "排序必须为正整数")]
        public int SortOrder { get; set; } 

        /// <summary>
        /// 广告位位置编号
        /// </summary>
        [Required(ErrorMessage = "广告位置不可为空")]
        [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "广告位必须为正整数")]
        public string AdPositionId { get; set; }
    }

    public class AdCurrencyPresentationModelWithId : AdCurrencyPresentationModel
    {
        [Required(ErrorMessage = "Id 不可为空")]
        public override int Id { get; set; }


    }

}
