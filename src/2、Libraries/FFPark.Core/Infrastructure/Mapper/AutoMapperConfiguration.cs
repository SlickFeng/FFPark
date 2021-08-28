using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFPark.Core.Infrastructure.Mapper
{
    /// <summary>
    /// 实体映射配置类
    /// </summary>
   public class AutoMapperConfiguration
    {
        public static IMapper Mapper { get; private set; }

        public static MapperConfiguration Configuration { get; set; }

        /// <summary>
        /// 初始化 mapper
        /// </summary>
        /// <param name="config"></param>
        public static void Init(MapperConfiguration config) 
        {
            Configuration = config;
            Mapper = config.CreateMapper();
        }
    }
}
