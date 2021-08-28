using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace FFPark.Core
{
    public interface IFFParkStartup
    {

        void ConfigureServices(IServiceCollection services, IConfiguration configuration);


        void Configure(IApplicationBuilder application);


        int Order { get; }
    }
}
