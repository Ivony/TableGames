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


    private static string host = ConfigurationManager.AppSettings["server"] ?? "http://game.jumony.net/";




    static void Main( string[] args )
    {

      var client = new GameClient( host );

      client.Run().Wait();


    }



  }
}
