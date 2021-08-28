using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFPark.Entity.Park;

namespace FFPark.Services.Park
{
    /// <summary>
    /// 车牌绑定
    /// </summary>
    public interface ILicenseBindServices
    {

        #region    小程序前端用

        /// 新增车牌绑定
        /// </summary>
        /// <param name="payChannel"></param>
        /// <returns></returns>
        Task<bool> AddLicenseBind(LicenseBind entity);

        /// <summary>
        /// 修改车牌绑定
        /// </summary>
        /// <param name="payChannel"></param>
        /// <returns></returns>
        Task<bool> UpdateLicenseBind(LicenseBind entity);



        Task<List<LicenseBind>> GetLicenseByOpenId(string license, string openId);

        #endregion

    }
}
