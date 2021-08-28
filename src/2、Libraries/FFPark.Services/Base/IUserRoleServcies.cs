using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFPark.Entity;

namespace FFPark.Services.Base
{
    public interface IUserRoleServcies
    {
        Task<bool> InsertUserRole(int userId, string roleIds);


        Task<bool> UpdateUserRole(int userId, string roleIds);

        /// <summary>
        /// 根据用户id 获取角色列表 字符串形式  rolename,rolename2
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns></returns>

        Task<string> GetRoleNames(int userId);

        Task<bool> SettingRole(int userId, int[] roleArray);
    }
}
