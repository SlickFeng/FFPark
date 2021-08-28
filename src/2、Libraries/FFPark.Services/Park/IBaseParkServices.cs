using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFPark.Entity.Park;
using FFPark.Model;
using FFPark.Model.Park;

namespace FFPark.Services.Park
{
    public interface IBaseParkServices : IBaseServices<BasePark>
    {
        Task<bool> IsExistParkName(string name, string fullName, int id);
        Task<IList<BaseParkConnModel>> GetBaseBarkConn();



        /// <summary>
        /// 新增车场
        /// </summary>
        /// <param name="park">车场基本信息</param>
        /// <param name="parkExtend">车场扩展信息</param>
        /// <returns></returns>
        Task<bool> Insert(BasePark park, BaseParkExtend parkExtend);

        /// <summary>
        /// 设置停车场管理员
        /// </summary>
        /// <param name="userid">管理员Id</param>
        /// <param name="parkid">车场Id</param>
        /// <returns></returns>
        Task<bool> SetParkAdmin(int userid, int parkid);

        /// <summary>
        /// 修改车场
        /// </summary>
        /// <param name="park">车场基本信息</param>
        /// <param name="parkExtend">车场扩展信息</param>
        /// <returns></returns>
        Task<bool> Update(BasePark park, BaseParkExtend parkExtend);

        /// <summary>
        /// 车场列表
        /// </summary>
        /// <param name="pageModel"></param>
        /// <returns></returns>
        Task<PageModel<BaseParkDto>> GetBasePariList(BaseParkQueryDto pageModel);
        /// <summary>
        /// 获取6个正在运行的停车场信息
        /// </summary>
        Task<IList<BaseParkDto>> GetTop6BaseParkList();
    }
}
