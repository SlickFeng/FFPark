using FreeSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FFPark.Services
{

    public static class IdleBusExtension
    {

      
        static AsyncLocal<string> asyncDb = new AsyncLocal<string>();

        public static IdleBus<IFreeSql> ChangeDatabase(this IdleBus<IFreeSql> ib, string db)
        {
            asyncDb.Value = db;
            return ib;
        }
        public static IFreeSql Get(this IdleBus<IFreeSql> ib) => ib.Get(asyncDb.Value ?? "db1");
        public static IBaseRepository<T> GetRepository<T>(this IdleBus<IFreeSql> ib) where T : class
            => ib.Get().GetRepository<T>();
        public static bool ValidDB(string connectionString)
        {
            IdleBus<IFreeSql> ib = new IdleBus<IFreeSql>(TimeSpan.FromMinutes(10));
            var fsql = ib.Get(); //获取当前租户对应的 IFreeSql
            var fsql00102 = ib.ChangeDatabase(connectionString).Get(); //切换租户，后面的操作都是针对 db2
            return true;
        }
    }
}
