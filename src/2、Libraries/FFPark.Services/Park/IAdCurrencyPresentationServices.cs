using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFPark.Entity.Park;
using FFPark.Model;

namespace FFPark.Services.Park
{
    public interface IAdCurrencyPresentationServices
    {

        #region    小程序前端用

        /// <summary>
        /// 获取通用广告
        /// </summary>
        /// <param name="channelName">通用广告名称</param>
        /// <returns></returns>
        Task<List<AdCurrencyPresentation>> GetAdCurrencyPresentationByName(string adName);

        #endregion


        #region   PC 后端用
        /// <summary>
        /// 新增通用广告
        /// </summary>
        /// <param name="payChannel"></param>
        /// <returns></returns>
        Task<bool> AddAdCurrencyPresentation(AdCurrencyPresentation entity);

        /// <summary>
        /// 删除通用广告-软删除
        /// </summary>
        /// <param name="id">通道编号</param>
        /// <param name="isDelete"></param>
        /// <returns></returns>
        Task<bool> DeleteAdCurrencyPresentation(int id, bool isDelete);

        /// <summary>
        /// 获取通道通过通道Id
        /// </summary>
        /// <param name="id">通道编号</param>
        /// <returns></returns>
        Task<AdCurrencyPresentation> GetAdCurrencyPresentationById(int id);



        /// <summary>
        /// 修改广告信息
        /// </summary>
        /// <param name="payChannel"></param>
        /// <returns></returns>
        Task<bool> UpdateAdCurrencyPresentation(AdCurrencyPresentation entity);

        /// <summary>
        /// 广告信息列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<PageModel<AdCurrencyPresentation>> GetAdCurrencyPresentationList(PageModelDto model);

        #endregion


    }
}
