using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFPark.Core.Extensions;
using FFPark.Data;
using FFPark.Entity.Park;
using FFPark.Model;
namespace FFPark.Services.Park
{
    public class AdCurrencyPresentationServices : IAdCurrencyPresentationServices
    {

        private readonly IRepository<AdCurrencyPresentation> _services;
        private readonly IFreeSql _freeSql;
        public AdCurrencyPresentationServices(IRepository<AdCurrencyPresentation> services, IFreeSql freeSql)
        {
            _services = services;
            _freeSql = freeSql;
        }

        #region    小程序前端用

        /// <summary>
        /// 获取通用广告
        /// </summary>
        /// <param name="channelName">通用广告名称</param>
        /// <returns></returns>
        public async Task<List<AdCurrencyPresentation>> GetAdCurrencyPresentationByName(string adName)
        {

            var result = await _freeSql.Select<AdCurrencyPresentation, AdPosition>()
                .LeftJoin((t1, t2) => t1.AdPositionId == t2.Id)
                .Where((t1, t2) => t2.PositionName.Contains(adName) || t2.Id.ToString() == adName)
                .OrderByDescending((t1, t2) => t1.SortOrder)
                .ToListAsync((t1, t2) => new AdCurrencyPresentation()
                {
                    AdImageUrl = t1.AdImageUrl,
                    SmallProgramTargetUrl = t1.SmallProgramTargetUrl,
                    TargetHttpUrl = t1.TargetHttpUrl
                });
            //  var result = await _services.GetAsync(t => t.LayerName == adName, t => t.SortOrder);
            return result;
        }
        #endregion

        #region   PC 后端用
        /// <summary>
        /// 新增通用广告
        /// </summary>
        /// <param name="payChannel"></param>
        /// <returns></returns>
        public async Task<bool> AddAdCurrencyPresentation(AdCurrencyPresentation entity)
        {
            var result = await _services.InsertAsync(entity);
            return result;
        }

        /// <summary>
        /// 删除通用广告-软删除
        /// </summary>
        /// <param name="id">通道编号</param>
        /// <param name="isDelete"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAdCurrencyPresentation(int id, bool isDelete)
        {
            var result = await _services.UpdateAsync(new AdCurrencyPresentation() { Id = id, IsDeleted = isDelete });
            return result;
        }

        /// <summary>
        /// 获取通道通过通道Id
        /// </summary>
        /// <param name="id">通道编号</param>
        /// <returns></returns>
        public async Task<AdCurrencyPresentation> GetAdCurrencyPresentationById(int id)
        {
            var result = await _services.GetAsync(t => t.Id == id);
            return result?.FirstOrDefault();
        }



        /// <summary>
        /// 修改广告信息
        /// </summary>
        /// <param name="payChannel"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAdCurrencyPresentation(AdCurrencyPresentation model)
        {
            var result = await _services.UpdateAsync(new AdCurrencyPresentation()
            {
                Id = model.Id,
                AdName = model.AdName,
                AdImageUrl = model.AdImageUrl,
                LayerName = model.LayerName,
                SmallProgramTargetUrl = model.SmallProgramTargetUrl,
                TargetHttpUrl = model.TargetHttpUrl,
                Remark = model.Remark
            });
            return result;
        }

        /// <summary>
        /// 广告信息列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<PageModel<AdCurrencyPresentation>> GetAdCurrencyPresentationList(PageModelDto model)
        {
            var result = new PageModel<AdCurrencyPresentation>();
            var list = await _freeSql.Select<AdCurrencyPresentation>()
                .Where(t => t.AdName.Contains(model.Key) || t.LayerName.Contains(model.Key))
                 .Count(out var total) //总记录数量
                 .Page(model.Page, model.PageSize)
                 .ToListAsync();
            result.Data = list;
            result.DataCount = Convert.ToInt32(total);
            result.PageSize = model.PageSize;
            result.PageCount = (Math.Ceiling(total.ObjToDecimal() / model.PageSize.ObjToDecimal())).ObjToInt();
            return result;
        }
        #endregion
    }
}
