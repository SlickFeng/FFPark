using FFPark.Entity.Park;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFPark.Services.Park
{
  public  interface IFastNavigationServices:IBaseServices<FastNavigation>
    {
        Task<IList<FastNavigation>> GetFastNavigationsByUserId(int userId);
    }
}
