using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FFPark.Data;
using FFPark.Entity;
using FFPark.Services.Customers;
using FFPark.Services.Security;

namespace FFPark.Services.Base
{
    public class BaseUserServices : IBaseUserServices
    {
        private IFreeSql _freeSql;

        private readonly IRepository<BaseUser> _baseUserrepository;
        /// <summary>
        /// 用户扩展仓储
        /// </summary>
        private readonly IRepository<UserExtension> _userExtensionRepository;

        private readonly IEncryptionService _encryptionService;





        public BaseUserServices(IEncryptionService encryptionService, IFreeSql freeSql, IRepository<BaseUser> baseUserrepository, IRepository<UserExtension> userExtensionRepository)
        {
            _baseUserrepository = baseUserrepository;
            _userExtensionRepository = userExtensionRepository;
            _freeSql = freeSql;
            _encryptionService = encryptionService;
        }

        public async Task<BaseUser> FindByIdAsync(int id)
        {
            var result = await _baseUserrepository.FindAsync(t => t.Id == id);
            return result;
        }

        public async Task<bool> FindByNameAsync(string userName)
        {
            var result = await _baseUserrepository.IsExistAsync(t => t.UserName == userName);
            return result;
        }

        public async Task<UserExtension> GetUserById(int userId)
        {
            var result = await _userExtensionRepository.GetOneAsyncWithColumn(t => t.UserId == userId, t => new UserExtension() { PetName = t.PetName, Avatar = t.Avatar });
            return result;
        }

        public async Task<bool> InsertBaseUser(BaseUser user, UserExtension userExtension)
        {
            //使用事务操作
            using (var now = _freeSql.CreateUnitOfWork())
            {
                var userEntityIdentity = (int)await _freeSql.Insert<BaseUser>(user).WithTransaction(now.GetOrBeginTransaction()).ExecuteIdentityAsync();
                if (userEntityIdentity <= 0)
                    return false;
                userExtension.UserId = userEntityIdentity;
                var result = await _freeSql.Insert<UserExtension>(userExtension).WithTransaction(now.GetOrBeginTransaction()).ExecuteAffrowsAsync();
                now.Commit();
                return result > 0 ? true : false;
            }
        }
        /// <summary>
        /// 代表第一次未授权的小程序登录获取token
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<BaseUser> SmallNoAuthenticate(string username, string password)
        {
            var result = await _baseUserrepository.FindAsync(t => t.UserName == username && t.UserPassWord == password && t.UserType == "100");
            return result;
        }
        public async Task<BaseUser> Login(string username, string password)
        {
            var result = await _baseUserrepository.FindAsync(t => t.UserName == username && t.UserPassWord == password);
            return result;
        }

        public async Task<bool> IsExistPetName(string petName, int id)
        {
            var result = await _userExtensionRepository.IsExistAsync(t => t.PetName == petName && (t.Id != id || id == 0));
            return result;
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="user">基础用户</param>
        /// <param name="userExtension">扩展用户</param>
        /// <returns></returns>
        public async Task<bool> UpdateBaseUser(BaseUser user, UserExtension userExtension)
        {
            using (var now = _freeSql.CreateUnitOfWork())
            {
                await _freeSql.Update<BaseUser>().WithTransaction(now.GetOrBeginTransaction()).SetSource(user).ExecuteAffrowsAsync();
                userExtension.UserId = user.Id;
                var result = await _freeSql.Update<UserExtension>().WithTransaction(now.GetOrBeginTransaction()).SetSource(userExtension).ExecuteAffrowsAsync();
                now.Commit();
                return result > 0 ? true : false;
            }
        }
    }
}
