using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFPark.Entity;
using FFPark.Model;

namespace FFPark.Services.Customers
{
    public interface IBaseCustomerServices
    {
        /// <summary>
        /// 增加一条微信小程序用户信息
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        int AddCustomer(BaseCustomer customer);

        /// <summary>
        /// 根据OpenId 查询用户信息
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        BaseCustomer GetBaseCustomerByOpenId(string openId);


        PageModel<BaseCustomer> GetBaseCustomer(PageModelDto pageModel);

    }
}
