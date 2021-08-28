using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FFPark.Core.Result;
using FFPark.Entity.Pay;
using FFPark.Model;
using FFPark.Services.Pay;
using FFPark.Web.Framework.Attributes;
using FFPark.Web.Framework.Controllers;
using FFPark.WebAPI.Infrastructure.Mapper.Extensions;

namespace FFPark.WebAPI.Controllers.Admin.Pay
{
    /// <summary>
    /// 后台-通道管理
    /// </summary>
    [ApiController]
    [Route("api/sys/pay/[action]")]
    [ApiAuthorize(Roles = "SystemAdmin")]
    [ApiExplorerSettings(GroupName = "pc")]
    public class PayChannelController : BaseAuthorizeController
    {

        private readonly IPayChannelServices _payChannelServices;
        public PayChannelController(IPayChannelServices payChannelServices)
        {
            _payChannelServices = payChannelServices;
        }
        /// <summary>
        /// 新增通道
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel> RaisePayChannel([FromBody] PayChannelModel model)
        {

            var payEntity = model.ToEntity<PayChannel>();
            var isName = await _payChannelServices.IsExistChannelByNamee(model.ChannelName, 0);
            if (isName) return FailedMsg("已存在名称为：" + model.ChannelName + "的通道");
            var result = await _payChannelServices.AddPayChannel(payEntity);
            if (result)
                return SuccessMsg("操作成功");
            else
                return SuccessMsg("操作失败");
        }

        /// <summary>
        /// 修改通道
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel> KeepPayChannel([FromBody] PayChannelModelWithId model)
        {
            var payEntity = model.ToEntity<PayChannel>();
            var isName = await _payChannelServices.IsExistChannelByNamee(model.ChannelName, model.Id);
            if (isName)
                if (isName) return FailedMsg("已存在名称为：" + model.ChannelName + "的通道");
            var result = await _payChannelServices.UpdatePayChannel(payEntity);
            if (result)
                return SuccessMsg("操作成功");
            else
                return SuccessMsg("操作失败");
        }
        /// <summary>
        /// 通道详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<PayChannelModel>> GetPayChannel(int id)
        {
            var entity = await _payChannelServices.GetPayChannelById(id);
            var model = entity.ToModel<PayChannelModel>();
            return Success(model);
        }

        /// <summary>
        /// 通道列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel<PageModel<PayChannel>>> GetPayChannelList([FromBody] PageModelDto model)
        {
            var result = await _payChannelServices.GetPayChannelList(model);
            return Success(result);
        }
    }
}
