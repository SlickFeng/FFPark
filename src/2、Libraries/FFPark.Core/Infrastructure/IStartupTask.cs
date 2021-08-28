using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFPark.Core.Infrastructure
{
    public interface IStartupTask
    {
        
        Task ExecuteAsync();

       
        int Order { get; }
    }
}
