using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFPark.Core.Infrastructure.Mapper
{
    public interface IOrderedMapperProfile
    {
        int Order { get; }
    }
}
