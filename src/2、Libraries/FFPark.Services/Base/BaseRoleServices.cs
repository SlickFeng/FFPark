using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFPark.Core.Extensions;
using FFPark.Data;
using FFPark.Entity;
using FFPark.Model;

namespace FFPark.Services.Base
{
    public class BaseRoleServices : IBaseRoleServices
    {

        private readonly IRepository<BaseRole> _baseRoleRepository;

        private IFreeSql _freeSql;

        public BaseRoleServices(IFreeSql freeSql, IRepository<BaseRole> baseRoleRepository)
        {
            _baseRoleRepository = baseRoleRepository;
            _freeSql = freeSql;
        }
        public async Task<bool> DeleteRole(int id, bool deleted)
        {
            var result = await _baseRoleRepository.UpdateAsync(new BaseRole() { Id = id, IsDeleted = deleted });
            return result;
        }

        public async Task<bool> FindByName(string roleName)
        {
            var result = await _baseRoleRepository.IsExistAsync(t => t.RoleName == roleName);
            return result;
        }

        public async Task<IList<BaseRole>> GetAllRole()
        {
            var result = await _baseRoleRepository.GetAsync(t => t.IsDeleted == true);
            return result;
        }
        public async Task<BaseRole> GetRoleById(int id)
        {
            var result = await _baseRoleRepository.GetByIdAsync(id);
            return result;
        }

        public async Task<bool> InsertRole(BaseRole entity)
        {
            var result = await _baseRoleRepository.InsertAsync(entity);
            return result;
        }

        public async Task<bool> IsExistRoleName(string roleName, int id)
        {
            var result = await _baseRoleRepository.IsExistAsync(t => t.RoleName == roleName && (t.Id != id || id == 0));
            return result;
        }

        public async Task<bool> UpdateRole(BaseRole entity)
        {
            var result = await _baseRoleRepository.UpdateAsync(new BaseRole() { Id = entity.Id, RoleDescription = entity.RoleDescription, RoleName = entity.RoleName, SortOrder = entity.SortOrder });
            return result;
        }

        public async Task<PageModel<BaseRole>> GetBaseRole(PageModelDto pageModel)
        {
            PageModel<BaseRole> page = new PageModel<BaseRole>();
            var list = _freeSql.Select<BaseRole>()
                .WhereIf(string.IsNullOrEmpty(pageModel.Key) == false, t => t.RoleDescription.Contains(pageModel.Key))
                .Count(out var total) //总记录数量
                .Page(pageModel.Page, pageModel.PageSize)
                .ToListAsync();
            page.Data = await list;
            page.DataCount = Convert.ToInt32(total);
            page.PageSize = pageModel.PageSize;
            page.PageCount = (Math.Ceiling(total.ObjToDecimal() / pageModel.PageSize.ObjToDecimal())).ObjToInt();
            return page;
        }

        /// <summary>
        /// 所有有效的角色列表
        /// </summary>
        /// <returns>Id RoleName 列表</returns>
        public async Task<List<BaseRole>> GetBaseRoleSelect()
        {
            var result = await _baseRoleRepository.GetAsyncWithColumn(t => t.IsDeleted == false, t => t.SortOrder, t => new BaseRole() { RoleName = t.RoleName, Id = t.Id });
            return result;
        }



    }
}
