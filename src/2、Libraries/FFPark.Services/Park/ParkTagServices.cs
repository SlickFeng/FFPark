using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFPark.Data;
using FFPark.Entity.Park;

namespace FFPark.Services.Park
{
    public class ParkTagServices : BaseServices<ParkTag>, IParkTagServices
    {

        private readonly IFreeSql _freeSql;
        private readonly IRepository<ParkTag> _services;

        public ParkTagServices(IRepository<ParkTag> services, IFreeSql freeSql) : base(services, freeSql)
        {
            _services = services;
            _freeSql = freeSql;
        }

        /// <summary>
        /// 验证车场tag 是否 超过五个，超过五个不允许新增
        /// </summary>
        /// <param name="parkId">车场Id</param>
        /// <returns></returns>
        public async Task<bool> CountFive(int parkId)
        {
            var result = await CountAsync(t => t.ParkId == parkId && t.IsDeleted == false);
            return result > 5 ? true : false;
        }
    }
}
