using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFPark.Core;
using FFPark.Data;
using FFPark.Entity;

namespace FFPark.Services.Base
{
    public class UserRoleServcies : IUserRoleServcies
    {

        private IFreeSql _freeSql;
        private readonly IRepository<BaseUserRole> _userRolerepository;
        private readonly IRepository<BaseRole> _roleRepository;


        public UserRoleServcies(IFreeSql freeSql, IRepository<BaseUserRole> userRolerepository, IRepository<BaseRole> roleRepository)
        {
            _userRolerepository = userRolerepository;
            _roleRepository = roleRepository;
            _freeSql = freeSql;
        }

        public async Task<bool> InsertUserRole(int userId, string roleIds)
        {

            int[] roleId = Array.ConvertAll<string, int>(roleIds.Split(','), s => int.Parse(s));
            if (!roleId.Any())
                throw new FFParkException("参数错误");
            List<BaseUserRole> baseUserRoles = new List<BaseUserRole>();
            roleId.AsParallel().ForAll(c => baseUserRoles.Add(new BaseUserRole() { UserId = userId, RoleId = c }));
            var result = await _userRolerepository.InsertAsync(baseUserRoles);
            return result > 0 ? true : false;
        }

        /// <summary>
        /// 修改用户角色
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public async Task<bool> UpdateUserRole(int userId, string roleIds)
        {
            using (var now = _freeSql.CreateUnitOfWork())
            {
                bool result = false;
                var deleted = await now.Orm.Delete<BaseUserRole>().WithTransaction(now.GetOrBeginTransaction()).Where(t => t.UserId == userId).ExecuteAffrowsAsync();
                if (deleted > 0)
                {
                    result = await InsertUserRole(userId, roleIds);
                }
                now.Commit();
                return result;
            }
        }
        /// <summary>
        /// 根据用户id 获取角色列表 字符串形式  rolename,rolename2
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns></returns>

        public async Task<string> GetRoleNames(int userId)
        {
            string roleNames = string.Empty;
            var userRole = await _userRolerepository.GetAsync(t => t.IsDeleted == false && t.UserId == userId);
            List<int> roleIds = new List<int>();
            if (userRole.Count > 0)
            {
                userRole.AsParallel().ForAll(c =>
                {
                    roleIds.Add(c.RoleId);
                });
                var role = await _roleRepository.GetAsync(t => t.IsDeleted == false && roleIds.Contains(t.Id));
                roleNames = string.Join(",", role.Select(r => r.RoleName).ToArray());
            }
            return roleNames;
        }

        public async Task<bool> SettingRole(int userId, int[] roleArray)
        {
            //删除原有角色信息
            await _userRolerepository.DeleteAsync(t => t.UserId == userId);
            //添加新的角色信息
            List<BaseUserRole> list = new List<BaseUserRole>();
            roleArray.AsParallel().ForAll(roleId =>
            {
                list.Add(new BaseUserRole() { RoleId = roleId, UserId = userId });
            });
            await _userRolerepository.InsertAsync(list);
            return true;
        }
    }
}
