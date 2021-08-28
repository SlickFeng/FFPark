using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeSql;
using FFPark.Entity;
using FFPark.Entity.Park;
using FFPark.Entity.Pay;
using FFPark.Entity.WeChat;

namespace FFPark.Services
{
    public class CodeFirstServices:ICodeFirstServices
    {
        private IFreeSql _freeSql;
        public CodeFirstServices(IFreeSql freeSql)
        {
            _freeSql = freeSql;
        }

        public void DBInitial()
        {
            _freeSql.CodeFirst.SyncStructure<BaseUser>();
            _freeSql.CodeFirst.SyncStructure<UserExtension>();
            _freeSql.CodeFirst.SyncStructure<BaseRole>();
            _freeSql.CodeFirst.SyncStructure<BaseUserRole>();
            _freeSql.CodeFirst.SyncStructure<BasePark>();
            _freeSql.CodeFirst.SyncStructure<BaseParkExtend>();
            _freeSql.CodeFirst.SyncStructure<AdCurrencyPresentation>();
            _freeSql.CodeFirst.SyncStructure<AdPosition>();
            _freeSql.CodeFirst.SyncStructure<CarSpecialDiscount>();
            _freeSql.CodeFirst.SyncStructure<LicenseBind>();
            _freeSql.CodeFirst.SyncStructure<ParkTag>();
            _freeSql.CodeFirst.SyncStructure<SpecialDiscount>();
            _freeSql.CodeFirst.SyncStructure<PayChannel>();
            _freeSql.CodeFirst.SyncStructure<WebChatConfig>();
            _freeSql.CodeFirst.SyncStructure<WebChatNotice>();
            _freeSql.CodeFirst.SyncStructure<BaseCustomer>();
            _freeSql.CodeFirst.SyncStructure<BaseNavigationModule>();     
        }
    }
}
