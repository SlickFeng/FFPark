using System.Collections.Generic;
using System.Threading.Tasks;
using FFPark.Entity;
using FFPark.Model;

namespace FFPark.Services.Base
{
    public interface IBaseUserServices
    {
        Task<BaseUser> Login(string username, string password);

        /// <summary>
        /// 小程序静默登录
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>

        Task<BaseUser> SmallNoAuthenticate(string username, string password);


        Task<UserExtension> GetUserById(int userId);

        Task<BaseUser> FindByIdAsync(int id);


        Task<bool> FindByNameAsync(string userName);


        Task<bool> InsertBaseUser(BaseUser user, UserExtension userExtension);

        Task<bool> IsExistPetName(string petName, int id);



        Task<bool> UpdateBaseUser(BaseUser user, UserExtension userExtension);



    }
}
