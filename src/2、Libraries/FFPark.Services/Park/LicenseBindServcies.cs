using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFPark.Data;
using FFPark.Entity.Park;

namespace FFPark.Services.Park
{
    public class LicenseBindServcies : ILicenseBindServices
    {
        #region    小程序前端用


        private readonly IRepository<LicenseBind> _services;
        private readonly IFreeSql _freeSql;
        public LicenseBindServcies(IRepository<LicenseBind> services, IFreeSql freeSql)
        {
            _services = services;
            _freeSql = freeSql;
        }
        /// 新增车牌绑定
        /// </summary>
        /// <param name="payChannel"></param>
        /// <returns></returns>
        public async Task<bool> AddLicenseBind(LicenseBind entity)
        {
            var result = await _services.InsertAsync(entity);
            return result;

        }

        /// <summary>
        /// 修改车牌绑定
        /// </summary>
        /// <param name="payChannel"></param>
        /// <returns></returns>
        public async Task<bool> UpdateLicenseBind(LicenseBind entity)
        {
            var result = await _services.UpdateAsync(a => new LicenseBind() { License = entity.License }, t => t.SmallProgramOpenId == entity.SmallProgramOpenId);
            return result;
        }

        public async Task<List<LicenseBind>> GetLicenseByOpenId(string license, string openId)
        {
            var result = await _services.GetAsync(t => t.License == license && t.SmallProgramOpenId == openId);
            return result;
        }
        #endregion
    }
}
