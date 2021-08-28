using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFPark.Entity.Park;
namespace FFPark.Services.Park
{
   public interface IParkTagServices:IBaseServices<ParkTag>
    {
        Task<bool> CountFive(int parkId);
    }
}
