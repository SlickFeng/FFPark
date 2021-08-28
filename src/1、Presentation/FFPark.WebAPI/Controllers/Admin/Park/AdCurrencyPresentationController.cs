using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using FFPark.Core.Result;
using FFPark.Entity.Park;
using FFPark.Model;
using FFPark.Services.Park;
using FFPark.Web.Framework.Attributes;
using FFPark.Web.Framework.Controllers;
using FFPark.WebAPI.Infrastructure.Mapper.Extensions;

namespace FFPark.WebAPI.Controllers.Admin.Park
{
    /// <summary>
    /// 后台-通用广告
    /// </summary>
    [ApiController]

    [Route("api/sys/ad/[action]")]


    [ApiAuthorize(Roles = "SystemAdmin")]


    [ApiExplorerSettings(GroupName = "pc")]
    public class AdCurrencyPresentationController : BaseAuthorizeController
    {
        private readonly IAdCurrencyPresentationServices _services;
        private readonly IAdPositionServices _adPositionServices;
        public AdCurrencyPresentationController(
            IAdCurrencyPresentationServices services,
            IAdPositionServices adPositionServices)
        {
            _services = services;
            _adPositionServices = adPositionServices;
        }
        /// <summary>
        /// 新增通用广告
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel> RaiseAdCurrencyPresentation([FromBody] AdCurrencyPresentationModel model)
        {

            var entity = model.ToEntity<AdCurrencyPresentation>();
            var isExistPositionId = await _adPositionServices.IsExistAdPositionId(model.AdPositionId);
            if (!isExistPositionId)
                return FailedMsg("无效的广告位置");
            var result = await _services.AddAdCurrencyPresentation(entity);
            if (result)
                return SuccessMsg("操作成功");
            else
                return FailedMsg("操作失败");
        }

        /// <summary>
        /// 修改通用广告
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel> KeepAdCurrencyPresentation([FromBody] AdCurrencyPresentationModelWithId model)
        {
            var entity = model.ToEntity<AdCurrencyPresentation>();
            var isExistPositionId = await _adPositionServices.IsExistAdPositionId(model.AdPositionId);
            if (isExistPositionId)
                return FailedMsg("无效的广告位置");
            var result = await _services.UpdateAdCurrencyPresentation(entity);
            if (result)
                return SuccessMsg("操作成功");
            else
                return SuccessMsg("操作失败");
        }
        /// <summary>
        /// 通用广告--详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<AdCurrencyPresentationModel>> GetAdCurrencyPresentation(int id)
        {
            var entity = await _services.GetAdCurrencyPresentationById(id);
            var model = entity.ToModel<AdCurrencyPresentationModel>();
            return Success(model);
        }

        /// <summary>
        /// 通用广告列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel<PageModel<AdCurrencyPresentation>>> GetAdCurrencyPresentation([FromBody] PageModelDto model)
        {
            var result = await _services.GetAdCurrencyPresentationList(model);
            return Success(result);
        }
    }
}
