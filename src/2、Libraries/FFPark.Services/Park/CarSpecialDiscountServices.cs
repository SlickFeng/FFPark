using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFPark.Data;
using FFPark.Entity.Park;

namespace FFPark.Services.Park
{
    public class CarSpecialDiscountServices : ICarSpecialDiscountServices
    {
        private readonly IRepository<CarSpecialDiscount> _baseRepository;

        private readonly IFreeSql _freeSql;

        public CarSpecialDiscountServices(IFreeSql freeSql, IRepository<CarSpecialDiscount> baseRepository)
        {
            _baseRepository = baseRepository;
            _freeSql = freeSql;
        }

        public async Task<bool> AddCarSpecialDiscount(CarSpecialDiscount entity)
        {
            var result = await _baseRepository.InsertAsync(entity);
            return result;
        }


        public async Task<bool> UseCarSpecialDiscount(int id, bool use)
        {
            var result = await _baseRepository.UpdateAsync(t => new CarSpecialDiscount() { IsDeleted = true }, t => t.Id == id);
            return result;
        }
    }
}
