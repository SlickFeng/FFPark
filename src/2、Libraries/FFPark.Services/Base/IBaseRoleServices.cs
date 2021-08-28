using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFPark.Entity;
using FFPark.Model;

namespace FFPark.Services.Base
{
    public interface IBaseRoleServices
    {
        Task<bool> FindByName(string roleName);


        Task<bool> InsertRole(BaseRole entity);


        Task<bool> DeleteRole(int id, bool deleted);


        Task<BaseRole> GetRoleById(int id);


        Task<IList<BaseRole>> GetAllRole();


        Task<bool> UpdateRole(BaseRole entity);


        Task<bool> IsExistRoleName(string roleName,int id);

        Task<PageModel<BaseRole>> GetBaseRole(PageModelDto pageModel);
    }
}
