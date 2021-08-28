using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFPark.Data;
using FFPark.Entity;
namespace FFPark.Services.Base
{
    public class BaseNavigationModuleServices : BaseServices<BaseNavigationModule>, IBaseNavigationModuleServices
    {
        private readonly IRepository<BaseNavigationModule> _services;

        private readonly IFreeSql _freeSql;
        public BaseNavigationModuleServices(IRepository<BaseNavigationModule> services, IFreeSql freeSql) : base(services, freeSql)
        {
            _services = services;
            _freeSql = freeSql;
        }
        public async Task<bool> FindByNameAsync(string userName)
        {
            var result = await _services.FindAsync(t => t.Name == userName);
            return result == null ? true : false;
        }
        public async Task<bool> IsExistTitle(string title, int id)
        {
            var result = await _services.IsExistAsync(t => t.Title == title && (t.Id != id || id == 0));
            return result;
        }

        public async Task<List<BaseNavigationModule>> GetNavigationModule()
        {
           var res= _freeSql.Select<BaseNavigationModule>().Where(t=>t.IsDeleted==false).ToTreeListAsync();

            var nav = await _services.GetAsync(t => t.IsDeleted == false, t => t.SortOrder);
            return nav;
        }
    }
}
