using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nebula.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ivony.TableGame.ConsoleClient
{

  public class MyHosting : ConsoleHosting
  {


    protected override void ConfigureServices( IHostEnvironment hostingEnvironment, IConfiguration configuration, IServiceCollection services )
    {

      services.AddSingleton<GameClient>();

      base.ConfigureServices( hostingEnvironment, configuration, services );
    }

  }

  class Program
  {

    static async Task Main(  )
    {

      await Hosting.RunAsync();

    }

  }
}
