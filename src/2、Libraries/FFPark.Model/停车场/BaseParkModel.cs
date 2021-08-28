using System.ComponentModel.DataAnnotations;
namespace FFPark.Model.Park
{
    /// <summary>
    /// 车场信息Model
    /// </summary>
    public class BaseParkModel : BaseFFParkModel
    {
        /// <summary>
        /// 停车场简称
        /// </summary>
        [Required(ErrorMessage = "停车场简称不可为为空")]
        [StringLength(10, ErrorMessage = "停车场简称长度不得超过10位")]
        public string ParkName { get; set; }
        /// <summary>
        /// 停车场全称
        /// </summary>
        [Required(ErrorMessage = "停车场全称不可为为空")]
        [StringLength(30, ErrorMessage = "停车场全称长度不得超过30位")]
        public string ParkFullName { get; set; }

        /// <summary>
        /// DB 连接字符串
        /// </summary>
        [Required(ErrorMessage = "停车场连接字符串不可为为空")]
        public string DBPath { get; set; }
        /// <summary>
        /// 车场信息扩展
        /// </summary>

        public BaseParkExtenModel Extend { get; set; }

    }
    /// <summary>
    /// 车场扩展Model
    /// </summary>
    public class BaseParkExtenModel : BaseFFParkModel
    {
        /// <summary>
        /// 车场所在省份
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 车场所在城市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 车场所在区域
        /// </summary>
        public string Region { get; set; }
        /// <summary>
        /// 车场所在街道
        /// </summary>
        public string Street { get; set; }
        /// <summary>
        /// 车场详细地址
        /// </summary>
        public string ParkAddress { get; set; }
    }

    /// <summary>
    /// 停车场信息列表Dto
    /// </summary>
    public class BaseParkDto : BaseParkExtenModel
    {
        public string ParkName { get; set; }

        public string ParkFullName { get; set; }


        public string DBPath { get; set; }



    }
    /// <summary>
    /// 车场查询Dto
    /// </summary>
    public class BaseParkQueryDto:PageModelDto
    {
        /// <summary>
        /// 车场名称
        /// </summary>
        public string ParkName { get; set; }
        /// <summary>
        /// 车场所在省份
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 车场所在城市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 车场所在区域
        /// </summary>
        public string Region { get; set; }
        /// <summary>
        /// 车场所在街道
        /// </summary>
        public string Street { get; set; }
        /// <summary>
        /// 车场详细地址
        /// </summary>
        public string ParkAddress { get; set; }

        /// <summary>
        /// 是否可用
        /// </summary>
        public bool IsDeleted { get; set; }
    }

    public class BaseParkConnModel
    {
        public string DBPath { get; set; }
        public string DBId { get; set; }
    }
}
