using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFPark.Core.Extensions;

namespace FFPark.Model
{
    public class SpecialDiscountModel : BaseFFParkModel, IValidatableObject
    {
        /// <summary>
        /// 优惠时间---单位小时
        /// </summary>
        [Required(ErrorMessage = "优惠时间不可为空"), Range(1, 24, ErrorMessage = "优惠时间在1--24 之间")]
        [RegularExpression(@"^(0|\+?[1-9][0-9]*)$", ErrorMessage = "优惠时间必须为正整数")]
        public string DiscountTime { get; set; }

        /// <summary>
        /// 优惠券开始时间
        /// </summary>
        [Required(ErrorMessage = "优惠券开始时间不可为空")]
        [RegularExpression("^((?!0000)[0-9]{4}-((0[1-9]|1[0-2])-(0[1-9]|1[0-9]|2[0-8])|(0[13-9]|1[0-2])-(29|30)|(0[13578]|1[02])-31)|([0-9]{2}(0[48]|[2468][048]|[13579][26])|(0[48]|[2468][048]|[13579][26])00)-02-29)$",
            ErrorMessage = "优惠券开始时间日期格式错误")]
        [DisplayFormat(DataFormatString = "yyyy-MM-dd HH:mm:ss")]
        public string BeginTime { get; set; }

        /// <summary>
        /// 优惠券结束时间
        /// </summary>
        [Required(ErrorMessage = "优惠券结束时间不可为空")]
        [RegularExpression("^((?!0000)[0-9]{4}-((0[1-9]|1[0-2])-(0[1-9]|1[0-9]|2[0-8])|(0[13-9]|1[0-2])-(29|30)|(0[13578]|1[02])-31)|([0-9]{2}(0[48]|[2468][048]|[13579][26])|(0[48]|[2468][048]|[13579][26])00)-02-29)$",
         ErrorMessage = "优惠券结束时间错误")]
        [DisplayFormat(DataFormatString = "yyyy-MM-dd HH:mm:ss")]

        public string EndTime { get; set; }   


        /// <summary>
        /// 优惠券名称
        /// </summary>
        [Required(ErrorMessage = "优惠券名称不可为空")]
        public string SpecialDiscountName { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            if (BeginTime.ToDateTime() < DateTime.Now)
            {
                results.Add(new ValidationResult("优惠券开始时间不能小于当前时间", new[] { "BeginTime" }));
            }

            if (EndTime.ToDateTime() < DateTime.Now)
            {
                results.Add(new ValidationResult("优惠券结束时间不能小于当前时间", new[] { "EndTime" }));
            }

            if (BeginTime.ToDateTime() > EndTime.ToDateTime())
            {
                results.Add(new ValidationResult("优惠券结束时间不能小于开始时间", new[] { "BeginTime" }));
            }
            return results;
        }
    }
}
