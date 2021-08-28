using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFPark.Model
{
    public class ParkTagModel : BaseFFParkModel
    {
        /// <summary>
        /// 车场ID
        /// </summary>
        public int ParkId { get; set; }
        /// <summary>
        /// 车场标签
        /// </summary>

        [Required(ErrorMessage = "车场标签不可为为空")]
        public string Title { get; set; }
    }
}
