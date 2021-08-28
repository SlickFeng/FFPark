using System.ComponentModel.DataAnnotations;

namespace FFPark.Model
{
    public partial class BaseFFParkEntityModel
    {
        /// <summary>
        /// Id 主键==>数据库主键
        /// </summary>
       // [RegularExpression(@"/^[1-9]d*$/", ErrorMessage = "编号必须为正整数")]
        public virtual int Id { get; set; }
    }
}
