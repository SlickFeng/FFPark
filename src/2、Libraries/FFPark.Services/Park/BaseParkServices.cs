using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFPark.Core.Extensions;
using FFPark.Data;
using FFPark.Entity.Park;
using FFPark.Model;
using FFPark.Model.Park;

namespace FFPark.Services.Park
{
    public class BaseParkServices : BaseServices<BasePark>, IBaseParkServices
    {
        private readonly IFreeSql _freeSql;
        private readonly IRepository<BasePark> _services;
        public BaseParkServices(IRepository<BasePark> services, IFreeSql freeSql) : base(services, freeSql)
        {
            _services = services;
            _freeSql = freeSql;
        }
        public async Task<bool> IsExistParkName(string name, string fullName, int id)
        {
            var result = await _services.FindAsync(t => t.ParkName == name || t.ParkFullName == fullName && (t.Id != id || id == 0));
            return result == null ? false : true;
        }
        public async Task<PageModel<BasePark>> GetBasePark(PageModelDto pageModel)
        {
            PageModel<BasePark> page = new PageModel<BasePark>();
            var list = _freeSql.Select<BasePark>()
                .WhereIf(string.IsNullOrEmpty(pageModel.Key) == false, t => t.ParkFullName.Contains(pageModel.Key) || t.ParkName.Contains(pageModel.Key))
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
        /// 新增车场
        /// </summary>
        /// <param name="park">车场基本信息</param>
        /// <param name="parkExtend">车场扩展信息</param>
        /// <returns></returns>
        public async Task<bool> Insert(BasePark park, BaseParkExtend parkExtend)
        {
            //使用事务操作
            using (var now = _freeSql.CreateUnitOfWork())
            {
                var parkEntityIdentity = (int)await _freeSql.Insert<BasePark>(park).WithTransaction(now.GetOrBeginTransaction()).ExecuteIdentityAsync();
                if (parkEntityIdentity <= 0)
                    return false;
                parkExtend.ParkId = parkEntityIdentity;
                var result = await _freeSql.Insert<BaseParkExtend>(parkExtend).WithTransaction(now.GetOrBeginTransaction()).ExecuteAffrowsAsync();
                now.Commit();
                return result > 0 ? true : false;
            }
        }

        /// <summary>
        /// 修改车场
        /// </summary>
        /// <param name="park">车场基本信息</param>
        /// <param name="parkExtend">车场扩展信息</param>
        /// <returns></returns>
        public async Task<bool> Update(BasePark park, BaseParkExtend parkExtend)
        {
            using (var now = _freeSql.CreateUnitOfWork())
            {
                var parkEntityIdentity = (int)await _freeSql.Update<BasePark>(new BasePark()
                {
                    DBPath = park.DBPath,
                    ParkFullName = park.ParkFullName,
                    ParkName = park.ParkName,
                    SortOrder = park.SortOrder
                }).WithTransaction(now.GetOrBeginTransaction()).ExecuteAffrowsAsync();
                if (parkEntityIdentity <= 0)
                    return false;
                parkExtend.ParkId = park.Id;
                var result = await _freeSql.Insert<BaseParkExtend>(parkExtend).WithTransaction(now.GetOrBeginTransaction()).ExecuteAffrowsAsync();
                now.Commit();
                return result > 0 ? true : false;
            }
        }

        /// <summary>
        /// 设置停车场管理员
        /// </summary>
        /// <param name="userid">管理员Id</param>
        /// <param name="parkid">车场Id</param>
        /// <returns></returns>
        public async Task<bool> SetParkAdmin(int userid, int parkid)
        {
            var result = await _services.UpdateAsync(t => new BasePark() { UserId = userid }, t => t.Id == parkid);
            return result;
        }

        public async Task<IList<BaseParkConnModel>> GetBaseBarkConn()
        {
            var result = await _services.GetAsyncWithColumn<BaseParkConnModel>(t => t.IsDeleted == false, t => t.SortOrder, x => new BaseParkConnModel() { DBPath = x.DBPath, DBId = "db" + x.Id });
            return result;
        }

        /// <summary>
        /// 获取6个正在运行的停车场信息
        /// </summary>
        public async Task<IList<BaseParkDto>> GetTop6BaseParkList()
        {
            var list = await _freeSql.Select<BasePark, BaseParkExtend>().LeftJoin((t1, t2) => t1.Id == t2.ParkId)
                  .Page(1, 6)
                .OrderByDescending(t => t.t1.SortOrder)
                .ToListAsync(t => new BaseParkDto()
                {
                    City = t.t2.City,
                    DBPath = t.t1.DBPath,
                    ParkFullName = t.t1.ParkFullName,
                    Id = t.t1.Id,
                    ParkAddress = t.t2.ParkAddress,
                    ParkName = t.t1.ParkName,
                    Province = t.t2.Province,
                    Region = t.t2.Region,
                    Street = t.t2.Street
                });
            return list;
        }


        /// <summary>
        /// 车场列表
        /// </summary>
        /// <param name="pageModel"></param>
        /// <returns></returns>
        public async Task<PageModel<BaseParkDto>> GetBasePariList(BaseParkQueryDto pageModel)
        {
            PageModel<BaseParkDto> page = new PageModel<BaseParkDto>();
            var list = _freeSql.Select<BasePark, BaseParkExtend>().LeftJoin((t1, t2) => t1.Id == t2.ParkId)
                .WhereIf(!string.IsNullOrEmpty(pageModel.Key), (t1, t2) => t1.ParkName.Contains(pageModel.Key) || t1.ParkFullName.Contains(pageModel.Key))
                .WhereIf(!string.IsNullOrEmpty(pageModel.Province), (t1, t2) => t2.Province == pageModel.Province)
                .WhereIf(!string.IsNullOrEmpty(pageModel.City), (t1, t2) => t1.ParkName.Contains(pageModel.City))
                .WhereIf(!string.IsNullOrEmpty(pageModel.Region), (t1, t2) => t1.ParkName.Contains(pageModel.Region))
                .WhereIf(!string.IsNullOrEmpty(pageModel.Street), (t1, t2) => t1.ParkName.Contains(pageModel.Street))
                .Count(out var total) //总记录数量
                .Page(pageModel.Page, pageModel.PageSize)
                .OrderByDescending(t => t.t1.SortOrder)
                .ToListAsync(t => new BaseParkDto()
                {
                    City = t.t2.City,
                    DBPath = t.t1.DBPath,
                    ParkFullName = t.t1.ParkFullName,
                    Id = t.t1.Id,
                    ParkAddress = t.t2.ParkAddress,
                    ParkName = t.t1.ParkName,
                    Province = t.t2.Province,
                    Region = t.t2.Region,
                    Street = t.t2.Street
                });
            page.Data = await list;
            page.DataCount = Convert.ToInt32(total);
            page.PageSize = pageModel.PageSize;
            page.PageCount = (Math.Ceiling(total.ObjToDecimal() / pageModel.PageSize.ObjToDecimal())).ObjToInt();
            return page;
        }
    }
}
