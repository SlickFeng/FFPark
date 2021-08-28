using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FFPark.Model
{
    /// <summary>
    /// 通用分页信息类
    /// </summary>
    public class PageModel<T>
    {
        /// <summary>
        /// 当前页标
        /// </summary>
        public int Page { get; set; } = 1;
        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount { get; set; } = 6;
        /// <summary>
        /// 数据总数
        /// </summary>
        public int DataCount { get; set; } = 0;
        /// <summary>
        /// 每页大小
        /// </summary>
        public int PageSize { set; get; }
        /// <summary>
        /// 返回数据
        /// </summary>
        public List<T> Data { get; set; }
    }

    public class PageModelDto
    {
        /// <summary>
        /// 当前页码
        /// </summary>
        [Required(ErrorMessage = "当前页码不可为0")]
        [Range(1, 99999999, ErrorMessage = "当前页码不可为0")]
        public int Page { get; set; } = 1;
        /// <summary>
        /// 每页显示数量
        /// </summary>
        [Required(ErrorMessage = "每页显示数量不可为0")]
        [Range(1, 99999999, ErrorMessage = "每页显示数量不可为0")]

        public int PageSize { get; set; } = 20;
        /// <summary>
        /// 搜索关键词
        /// </summary>

        public string Key { get; set; } = "";

        /// <summary>
        /// 是否启用   999 代表全部, 0  启用 , 1  禁用
        /// </summary>
        public int Flag { get; set; } = 999;
    }



    public class PageModelWithIdDto : PageModelDto
    {
        public int Id { get; set; }
    }
}
