using FFPark.Core;
using FreeSql.DataAnnotations;

namespace FFPark.Entity.Park
{
    [Table(Name = "BasePark")]
    public class BasePark : BaseEntity
    {
        /// <summary>
        /// 停车场简称
        /// </summary>
        public string ParkName { get; set; }
        /// <summary>
        /// 停车场全称
        /// </summary>
        public string ParkFullName { get; set; }

        /// <summary>
        /// DB 连接字符串
        /// </summary>
        public string DBPath { get; set; }

        /// <summary>
        /// 管理员UserId
        /// </summary>
        public int UserId { get;set;}
    }
}
