using FreeSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFPark.Core.Configuration
{
    public partial class DataConfig:BaseEntity
    {
        #region 构造
        public DataConfig()
        {

        }
        #endregion

        #region 属性
        public string MasterConnetion { get; set; }


        public DataType DataType { get; set; }

        /// <summary>
        /// 从库链接
        /// </summary>
        public List<SlaveConnection> SlaveConnections { get; set; }
        #endregion
    }
    public class SlaveConnection
    {
        public string ConnectionString { get; set; }
    }
}
