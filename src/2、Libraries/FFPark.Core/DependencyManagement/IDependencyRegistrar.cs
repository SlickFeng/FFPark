using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFPark.Core.Configuration;

namespace FFPark.Core.Infrastructure.DependencyManagement
{
    public interface IDependencyRegistrar
    {

        void Register(ContainerBuilder builder, ITypeFinder typeFinder, AppSettings appSettings);


        int Order { get; }
    }
}
