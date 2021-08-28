using System;
using System.Collections.Generic;
using System.Text;
using FreeSql.DataAnnotations;
namespace FFPark.Core
{
    public abstract partial class BaseEntity
    {
        /// <summary>
        /// 主键自增Id
        /// </summary>
        [Column(IsPrimary = true, IsIdentity = true)]
        public virtual int Id { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public virtual bool IsDeleted { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime => DateTime.Now;

        /// <summary>
        /// 排序字段，越大越靠前
        /// </summary>
        public virtual int SortOrder { get; set; }
    }
}
