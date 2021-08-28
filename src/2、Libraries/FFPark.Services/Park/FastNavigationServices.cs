using FFPark.Data;
using FFPark.Entity.Park;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFPark.Services.Park
{
    public class FastNavigationServices : BaseServices<FastNavigation>, IFastNavigationServices
    {
        private readonly IFreeSql _freeSql;
        private readonly IRepository<FastNavigation> _services;
        public FastNavigationServices(IRepository<FastNavigation> services, IFreeSql freeSql) : base(services, freeSql)
        {
            _services = services;
            _freeSql = freeSql;
        }

        public async Task<IList<FastNavigation>> GetFastNavigationsByUserId(int userId)
        {
            var result = await _services.GetAsync(t => t.UserId == userId);
            return result;
        }
    }
}
