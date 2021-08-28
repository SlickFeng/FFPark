using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFPark.Entity;
namespace FFPark.Services.Base
{
    public interface IBaseNavigationModuleServices : IBaseServices<BaseNavigationModule>
    {
        Task<bool> FindByNameAsync(string userName);
        Task<bool> IsExistTitle(string title, int id);

        /// <summary>
        /// 左侧菜单列表
        /// </summary>
        /// <returns></returns>
        Task<List<BaseNavigationModule>> GetNavigationModule();
    }
}
