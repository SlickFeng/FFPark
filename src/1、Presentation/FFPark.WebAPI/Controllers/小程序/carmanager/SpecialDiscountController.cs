using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FFPark.Core.Result;
using FFPark.Entity.Park;
using FFPark.Model;
using FFPark.Services.Park;
using FFPark.Web.Framework.Controllers;
using FFPark.WebAPI.Infrastructure.Mapper.Extensions;

namespace FFPark.WebAPI.Controllers
{
    /// <summary>
    ///车主/PC 管理优惠券
    /// </summary>
    [ApiController]
    [Route("api/wx/SpecialDiscount/[action]")]
    [ApiExplorerSettings(GroupName = "carmanager")]
    public class SpecialDiscountController : BaseAuthorizeController
    {
        private readonly ISpecialDiscountServices _specialDiscountServices;
        public SpecialDiscountController(
            ISpecialDiscountServices specialDiscountServices)
        {
            _specialDiscountServices = specialDiscountServices;
        }

        /// <summary>
        /// 新增优惠券
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel> RaiseDiscount([FromBody] SpecialDiscountModel model)
        {
            var entity = model.ToEntity<SpecialDiscount>();
            entity.ParkId = 1;
            var list = await _specialDiscountServices.AddSpecialDiscount(entity);
            if (list)
                return SuccessMsg("操作成功");
            return FailedMsg("操作失败");
        }
    }
}
