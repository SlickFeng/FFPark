using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFPark.Entity.Park;
namespace FFPark.Services.Park
{
    public interface IAdPositionServices
    {
        Task<bool> AdPosition(AdPosition entity);

        Task<List<AdPosition>> GetAdPositions();



        Task<bool> IsExistAdPositionName(string name);

        Task<bool> IsExistAdPositionId(string id);
    }
}
