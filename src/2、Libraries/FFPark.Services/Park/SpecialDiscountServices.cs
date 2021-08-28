using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFPark.Core.Extensions;
using FFPark.Data;
using FFPark.Entity.Park;
using FFPark.Model;
namespace FFPark.Services.Park
{
    /// <summary>
    /// 停车场发行优惠券接口
    /// </summary>
    public class SpecialDiscountServices : ISpecialDiscountServices
    {

        private readonly IRepository<SpecialDiscount> _baseRepository;

        private readonly IFreeSql _freeSql;

        public SpecialDiscountServices(IFreeSql freeSql, IRepository<SpecialDiscount> baseRepository)
        {
            _baseRepository = baseRepository;
            _freeSql = freeSql;
        }

        public async Task<bool> AddSpecialDiscount(SpecialDiscount entity)
        {
            var result = await _baseRepository.InsertAsync(entity);
            return result;
        }



        public async Task<bool> UpdateSpecialDiscount(SpecialDiscount entity)
        {
            var result = await _baseRepository.UpdateAsync(new SpecialDiscount()
            {
                BeginTime = entity.BeginTime,
                EndTime = entity.EndTime,
                IsDeleted = entity.IsDeleted,
                DiscountTime = entity.DiscountTime
            });
            return result;
        }



        public async Task<PageModel<SpecialDiscount>> GetSpecialDiscount(PageModelDto pageModel)
        {
            PageModel<SpecialDiscount> page = new PageModel<SpecialDiscount>();
            var list = _freeSql.Select<SpecialDiscount>()
                .WhereIf(string.IsNullOrEmpty(pageModel.Key) == false, t => t.SpecialDiscountName.Contains(pageModel.Key))
                .Count(out var total) //总记录数量
                .Page(pageModel.Page, pageModel.PageSize)
                .ToListAsync();
            page.Data = await list;
            page.DataCount = Convert.ToInt32(total);
            page.PageSize = pageModel.PageSize;
            page.PageCount = (Math.Ceiling(total.ObjToDecimal() / pageModel.PageSize.ObjToDecimal())).ObjToInt();
            return page;
        }



        public async Task<SpecialDiscount> GetSpecialDiscount(int id)
        {
            var result = await _baseRepository.GetByIdAsync(id);
            return result;
        }
    }
}
