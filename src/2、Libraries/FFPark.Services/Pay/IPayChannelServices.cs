using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFPark.Entity.Pay;
using FFPark.Model;

namespace FFPark.Services.Pay
{
    public interface IPayChannelServices
    {
        /// <summary>
        /// 新增通道
        /// </summary>
        /// <param name="payChannel"></param>
        /// <returns></returns>
        Task<bool> AddPayChannel(PayChannel payChannel);

        /// <summary>
        /// 删除通道-软删除
        /// </summary>
        /// <param name="id">通道编号</param>
        /// <param name="isDelete"></param>
        /// <returns></returns>
        Task<bool> DeletePayChannel(int id, bool isDelete);

        /// <summary>
        /// 获取通道通过通道Id
        /// </summary>
        /// <param name="id">通道编号</param>
        /// <returns></returns>
        Task<PayChannel> GetPayChannelById(int id);

        /// <summary>
        /// 获取通道数据
        /// </summary>
        /// <param name="channelName">通道名称</param>
        /// <returns></returns>
        Task<PayChannel> GetPayChannelByName(string channelName);

        /// <summary>
        /// 是否存在通道
        /// </summary>
        /// <param name="petName">通道名称</param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> IsExistChannelByNamee(string petName, int id);

        Task<bool> UpdatePayChannel(PayChannel payChannel);

        Task<PageModel<PayChannel>> GetPayChannelList(PageModelDto model);

    }
}
