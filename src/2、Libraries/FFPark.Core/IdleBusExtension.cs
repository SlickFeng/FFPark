using FreeSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FFPark.Core
{
    public enum DbName { db0, db1, db2 }
    //public class IdleBusExtension : IdleBus<DbName, IFreeSql>
    //{
    //    public IdleBusExtension() : base(TimeSpan.FromMinutes(30)) { }

    //    public static bool ValidDB(string connection)
    //    {
    //        IdleBusExtension ib = new IdleBusExtension();
    //        var result = ib.Register(DbName.db2, () => new FreeSqlBuilder().UseConnectionString(DataType.SqlServer, "connection").Build());
    //        int count = result.Quantity;
    //        if (count > 0) return true;
    //        result.Dispose();
    //        return false;
    //    }
    //}

   


}
