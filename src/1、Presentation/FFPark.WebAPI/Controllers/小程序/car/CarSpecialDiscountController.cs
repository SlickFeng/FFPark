using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FFPark.Core.Result;
using FFPark.Entity.Park;
using FFPark.Services.Park;
using FFPark.Web.Framework.Controllers;

namespace FFPark.WebAPI.Controllers
{
    /// <summary>
    /// 车主小程序 -领取优惠券
    /// </summary>
    [ApiController]
    [Route("api/wx/CarSpecialDiscount/[action]")]
    [ApiExplorerSettings(GroupName = "car")]
    public class CarSpecialDiscountController : BaseAuthorizeController
    {

        private readonly ICarSpecialDiscountServices _carSpecialDiscountServices;
        private readonly ISpecialDiscountServices _specialDiscountServices;
        public CarSpecialDiscountController(
            ICarSpecialDiscountServices carSpecialDiscountServices,
            ISpecialDiscountServices specialDiscountServices)
        {
            _carSpecialDiscountServices = carSpecialDiscountServices;
            _specialDiscountServices = specialDiscountServices;
        }

        /// <summary>
        /// 车主通过小程序领取优惠券
        /// </summary>
        /// <param name="id">优惠券Id</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel> PullDown(int id)
        {

            var carSpecialDiscount = await _specialDiscountServices.GetSpecialDiscount(id);
            if (carSpecialDiscount == null)
                return FailedMsg("该优惠券不存在");
            if (carSpecialDiscount.EndTime < DateTime.Now.AddMinutes(10))
                return FailedMsg("该优惠券已过有效期");
            CarSpecialDiscount entity = new CarSpecialDiscount()
            {
                BeginTime = carSpecialDiscount.BeginTime,
                EndTime = carSpecialDiscount.EndTime,
                DiscountTime = carSpecialDiscount.DiscountTime,
                ParkId = carSpecialDiscount.ParkId,
                SmallOpenId = "oTQRQ5Jt8rTOcbJsjxpCl7o1JlKg"
            };
            var result = await _carSpecialDiscountServices.AddCarSpecialDiscount(entity);
            if (result)
                return SuccessMsg("领取成功");
            return FailedMsg("领取失败");
        }


    }
}
