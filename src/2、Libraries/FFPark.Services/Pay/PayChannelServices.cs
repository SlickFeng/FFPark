using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFPark.Core.Extensions;
using FFPark.Data;
using FFPark.Entity.Pay;
using FFPark.Model;

namespace FFPark.Services.Pay
{
    /// <summary>
    /// 支付通道
    /// </summary>
    public class PayChannelServices : IPayChannelServices
    {

        private readonly IRepository<PayChannel> _services;
        private readonly IFreeSql _freeSql;
        public PayChannelServices(IRepository<PayChannel> services, IFreeSql freeSql)
        {
            _services = services;
            _freeSql = freeSql;
        }

        /// <summary>
        /// 新增通道
        /// </summary>
        /// <param name="payChannel"></param>
        /// <returns></returns>
        public async Task<bool> AddPayChannel(PayChannel payChannel)
        {
            var result = await _services.InsertAsync(payChannel);
            return result;
        }

        /// <summary>
        /// 删除通道-软删除
        /// </summary>
        /// <param name="isDelete"></param>
        /// <returns></returns>
        public async Task<bool> DeletePayChannel(int id, bool isDelete)
        {
            var result = await _services.UpdateAsync(new PayChannel() { Id = id, IsDeleted = isDelete });
            return result;
        }

        /// <summary>
        /// 获取通道通过通道Id
        /// </summary>
        /// <param name="id">通道编号</param>
        /// <returns></returns>
        public async Task<PayChannel> GetPayChannelById(int id)
        {
            var result = await _services.GetAsync(t => t.Id == id);
            return result?.FirstOrDefault();
        }

        /// <summary>
        /// 是否存在通道
        /// </summary>
        /// <param name="petName">通道名称</param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> IsExistChannelByNamee(string petName, int id)
        {
            var result = await _services.IsExistAsync(t => t.ChannelName == petName && (t.Id != id || id == 0));
            return result;
        }

        /// <summary>
        /// 获取通道数据
        /// </summary>
        /// <param name="channelName">通道名称</param>
        /// <returns></returns>
        public async Task<PayChannel> GetPayChannelByName(string channelName)
        {
            var result = await _services.GetAsync(t => t.Remark.Contains(channelName) || t.ChannelFullName.Contains(channelName) || t.ChannelName.Contains(channelName));
            return result?.FirstOrDefault();
        }

        public async Task<bool> UpdatePayChannel(PayChannel payChannel)
        {
            var result = await _services.UpdateAsync(new PayChannel()
            {
                Id = payChannel.Id,
                ChannelName = payChannel.ChannelName,
                ChannelFullName = payChannel.ChannelFullName,
                Remark = payChannel.Remark
            });
            return result;
        }


        public async Task<PageModel<PayChannel>> GetPayChannelList(PageModelDto model)
        {

            var result = new PageModel<PayChannel>();
            var list = await _freeSql.Select<PayChannel>()
                .Where(t => t.ChannelFullName.Contains(model.Key) || t.ChannelName.Contains(model.Key) || t.Remark.Contains(model.Key))
                 .Count(out var total) //总记录数量
                 .Page(model.Page, model.PageSize)
                 .ToListAsync();
            result.Data =  list;
            result.DataCount = Convert.ToInt32(total);
            result.PageSize = model.PageSize;
            result.PageCount = (Math.Ceiling(total.ObjToDecimal() / model.PageSize.ObjToDecimal())).ObjToInt();
            return result;
        }
    }
}
