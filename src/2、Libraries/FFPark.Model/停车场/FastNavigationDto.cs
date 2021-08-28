using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFPark.Model
{
    public class FastNavigationDto
    {
        public string NavigationUrl { get; set; }
        /// <summary>
        /// 提示
        /// </summary>
        public string Title { get; set; }
    }

    public class FastNavigationModel : BaseFFParkModel
    {

        public string NavigationUrl { get; set; }
        /// <summary>
        /// 提示
        /// </summary>
        public string Title { get; set; }

        public int UserId { get; set; }
    }
}
