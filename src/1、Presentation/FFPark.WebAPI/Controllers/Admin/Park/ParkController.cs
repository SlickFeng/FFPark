using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using FFPark.Core;
using FFPark.Core.Result;
using FFPark.Entity.Park;
using FFPark.Model;
using FFPark.Model.Park;
using FFPark.Services.Base;
using FFPark.Services.Park;
using FFPark.Web.Framework.Attributes;
using FFPark.Web.Framework.Controllers;
using FFPark.WebAPI.Infrastructure.Mapper.Extensions;
using System.Collections.Generic;

namespace FFPark.WebAPI.Controllers.Admin.Park
{
    /// <summary>
    /// 后台-车场管理
    /// </summary>
    [ApiController]
    [Route("api/park/[action]")]
    [ApiAuthorize(Roles = "SystemAdmin")]
    [ApiExplorerSettings(GroupName = "pc")]
    public class ParkController : BaseAuthorizeController
    {

        private readonly IBaseParkServices _baseParkServices;

        public ParkController(IBaseParkServices baseParkServices)
        {
            _baseParkServices = baseParkServices;
        }
        /// <summary>
        /// 新增车场
        /// </summary>
        /// <param name="model"></param>                             
        /// <returns></returns>                   
        [HttpPost]
        public async Task<MessageModel> RaisePark([FromBody] BaseParkModel model)
        {
            var parkEntity = model.ToEntity<BasePark>();

            //测试车场连接字符串是否正确
            //省略了改代码
            var isExistParkName = await _baseParkServices.IsExistParkName(model.ParkName, model.ParkFullName, 0);
            if (isExistParkName)
                return FailedMsg("已存在" + model.ParkName + "或" + model.ParkFullName);
            //var successConnection = IdleBusExtension.ValidDB(model.DBPath);
            //if (!successConnection)
            //    return FailedMsg("连接字符串无法连接，请检查后重试");
            var extendPark = model.Extend.ToEntity<BaseParkExtend>();
            var result = await _baseParkServices.Insert(parkEntity, extendPark);
            if (result)
                return SuccessMsg("操作成功");
            else
                return FailedMsg("操作失败");
        }

        /// <summary>
        /// 修改车场
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel> KeepPark([FromBody] BaseParkModel model)
        {

            var isExistRoleName = await _baseParkServices.IsExistParkName(model.ParkName, model.ParkFullName, model.Id);
            if (isExistRoleName)
                return FailedMsg("已存在" + model.ParkName + "或" + model.ParkFullName);
            var parkEntity = model.ToEntity<BasePark>();
            var extendPark = model.Extend.ToEntity<BaseParkExtend>();
            var result = await _baseParkServices.Update(parkEntity, extendPark);
            if (result)
                return SuccessMsg("操作成功");
            else
                return FailedMsg("操作失败");
        }
        /// <summary>
        /// 设置停车场管理员
        /// </summary>
        /// <param name="userid">停车场管理员id</param>
        /// <param name="parkid">停车场Id</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel> SetAdminPark(int userid, int parkid)
        {
            var result = await _baseParkServices.SetParkAdmin(userid, parkid);
            return SuccessMsg("操作成功");
        }


        /// <summary>
        /// 车场列表
        /// </summary>
        /// <param name="pageModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<MessageModel<PageModel<BaseParkDto>>> GetBaseParkList(BaseParkQueryDto pageModel)
        {
            var result = await _baseParkServices.GetBasePariList(pageModel);
            return Success(result);
        }

        /// <summary>
        /// 获取6个正在运行的停车场信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<IList<BaseParkDto>>> GetTop6BaseParkList()
        {
            var result = await _baseParkServices.GetTop6BaseParkList();
            return Success(result);
        }
    }
}
