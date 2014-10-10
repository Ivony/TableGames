using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.ConsoleClient
{
  class Program
  {


    private static string host = "http://localhost:32800/";




    static void Main( string[] args )
    {


      var cookieContainer = new CookieContainer();

      using ( var handler = new HttpClientHandler() { CookieContainer = cookieContainer } )
      {

        using ( var client = new HttpClient( handler ) )
        {




          Process( client ).Wait();

        }
      }


    }


    private static async Task Process( HttpClient client )
    {
      Console.Write( "请输入要加入的游戏的名称：" );
      var name = Console.ReadLine();


      await client.GetAsync( host + "Game?name=" + name );


      while ( true )
      {

        var status = await GetStatus( client );

        foreach ( var message in status.Messages )
        {

          Console.WriteLine( "[{0}] {1}", message.Date, message.Message );

        }
      }
    }

    private async static Task<dynamic> GetStatus( HttpClient client )
    {
      var response = await client.GetAsync( host );

      return JObject.Parse( await response.Content.ReadAsStringAsync() );
    }

  }
}
