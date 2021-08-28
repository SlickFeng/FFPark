using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFPark.Entity.Park;
using FFPark.Model;

namespace FFPark.Services.Park
{
    /// <summary>
    /// 优惠券接口
    /// </summary>
    public interface ISpecialDiscountServices
    {
        Task<bool> AddSpecialDiscount(SpecialDiscount entity);



        Task<bool> UpdateSpecialDiscount(SpecialDiscount entity);



        Task<PageModel<SpecialDiscount>> GetSpecialDiscount(PageModelDto pageModel);



        Task<SpecialDiscount> GetSpecialDiscount(int id);
    }
}
