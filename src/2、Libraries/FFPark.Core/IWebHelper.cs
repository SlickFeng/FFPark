using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFPark.Core
{
    public partial interface IWebHelper
    {

        string GetUrlReferrer();

        string GetCurrentIpAddress();

        void RestartAppDomain();
    }
}
