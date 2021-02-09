using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nebula.Hosting;

namespace Ivony.TableGame.WebHost
{
  public class MyHosting : MvcHosting
  {




    protected override void ConfigureServices( IHostEnvironment hostingEnvironment, IConfiguration configuration, IServiceCollection services )
    {
      services.AddSingleton<PlayerHostMiddleware>();
      base.ConfigureServices( hostingEnvironment, configuration, services );
    }

    protected override void ConfigureWebApplication( IApplicationBuilder builder, IConfiguration configuration, ILogger logger )
    {
      builder.UseMiddleware<PlayerHostMiddleware>();

      base.ConfigureWebApplication( builder, configuration, logger );
    }

  }
}
