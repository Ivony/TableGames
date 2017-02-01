using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ivony.TableGame.ConsoleClient
{
  class Program
  {




    static void Main( string[] args )
    {
      var url = new Uri( ConfigurationManager.AppSettings["server"] ?? "http://game.jumony.net/" );

      var client = new GameClient( url );

      client.Run().Wait();


    }



  }
}
