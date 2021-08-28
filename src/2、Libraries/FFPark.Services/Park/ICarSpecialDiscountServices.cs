using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFPark.Entity.Park;
namespace FFPark.Services.Park
{
    public interface ICarSpecialDiscountServices
    {
        Task<bool> AddCarSpecialDiscount(CarSpecialDiscount entity);

        Task<bool> UseCarSpecialDiscount(int id,bool use);



    }
}
