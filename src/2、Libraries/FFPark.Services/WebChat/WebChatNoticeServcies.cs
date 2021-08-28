using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFPark.Data;
using FFPark.Entity.WeChat;

namespace FFPark.Services.WebChat
{
    public class WebChatNoticeServcies : IWebChatNoticeServices
    {

        private readonly IRepository<WebChatNotice> _services;

        private readonly IFreeSql _freeSql;

        public WebChatNoticeServcies(IRepository<WebChatNotice> services, IFreeSql freeSql)
        {
            _services = services;
            _freeSql = freeSql;
        }

        public async Task<bool> AddWebChatNotice(IList<WebChatNotice> model)
        {
            //删除数据


            using (var now = _freeSql.CreateUnitOfWork())
            {

                var deleted = await now.Orm.Delete<WebChatNotice>().Where(t => t.IsDeleted == false).ExecuteAffrowsAsync();
                int rows = await now.Orm.Insert(model.AsEnumerable()).ExecuteAffrowsAsync();
                now.Commit();
                return true;
            }


            //var result = await _services.InsertAsync(model);
            //return result > 0 ? true : false;
        }

        public async Task<List<WebChatNotice>> GetWebChatNoticeByTitle(List<string> title)
        {
            var result = await _services.GetAsyncWithColumn(t => title.Contains(t.Title), t => new WebChatNotice() { TemplateId = t.TemplateId, Title=t.Title });
            //var result = await _freeSql.Select<WebChatNotice>().Where(t => title.Contains(t.Title)).ToListAsync(t => new WebChatNotice() { Title = t.Title });
            return result;
        }
    }
}
