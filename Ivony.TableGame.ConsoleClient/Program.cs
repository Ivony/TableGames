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


      while ( true )
      {
        var lastRequestTime = DateTime.UtcNow;


        try
        {

          var status = await GetStatus( client );

          foreach ( var message in status.Messages )
          {

            switch ( (string) message.Type )
            {
              case "System":
                Console.ForegroundColor = ConsoleColor.Cyan;
                break;

              case "Warning":
                Console.ForegroundColor = ConsoleColor.Yellow;
                break;

              case "Error":
                Console.ForegroundColor = ConsoleColor.Red;
                break;

              case "SystemError":
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.BackgroundColor = ConsoleColor.DarkCyan;
                break;


            }

            Console.WriteLine( "[{0}] {1}", message.Date, message.Message );

            Console.ForegroundColor = ConsoleColor.Gray;

          }

          if ( status.Gaming == false )
          {
            Console.Write( "您当前尚未加入游戏，请输入要加入的游戏房间名：" );
            var name = Console.ReadLine();

            var source = new CancellationTokenSource( new TimeSpan( 0, 0, 10 ) );
            await client.GetAsync( host + "Game?name=" + name, source.Token );
            continue;
          }


          var compatibility = ((JArray) status.Compatibility).ToObject<string[]>();
          if ( compatibility.Contains( "Console", StringComparer.OrdinalIgnoreCase ) == false )
          {


            Console.Write( "游戏不兼容控制台客户端，正在退出" );
            QuitGame( client );
            continue;
          }


          if ( status.WaitForResponse == true )
          {
            Console.Beep();
            Console.Write( status.PromptText );
            var message = Console.ReadLine();
            await SendResponse( client, message );
            continue;
          }

          else
          {
            var delay = lastRequestTime.AddSeconds( 1 ) - DateTime.UtcNow;

            if ( delay > TimeSpan.Zero )
              await Task.Delay( delay );
          }
        }
        catch ( TaskCanceledException )
        {
          Console.ForegroundColor = ConsoleColor.Red;
          Console.WriteLine( "连接服务器超时，尝试重新连接" );
          Console.ForegroundColor = ConsoleColor.Gray;
        }
        catch ( Exception e )
        {
          Console.ForegroundColor = ConsoleColor.Red;
          Console.WriteLine( "出现异常：" );
          Console.WriteLine( e );
          Console.WriteLine( "5秒后尝试重新连接服务器" );
          Thread.Sleep( new TimeSpan( 0, 0, 5 ) );
          Console.WriteLine( "尝试重新连接" );
          Console.ForegroundColor = ConsoleColor.Gray;
        }
      }
    }

    private async static void QuitGame( HttpClient client )
    {
      await client.GetAsync( host + "/QuitGame" );
    }

    private async static Task<dynamic> GetStatus( HttpClient client )
    {
      var source = new CancellationTokenSource( new TimeSpan( 0, 0, 10 ) );

      var response = await client.GetAsync( host, source.Token );

      if ( response.StatusCode != HttpStatusCode.OK )
        throw new Exception( "访问服务器出现错误" );

      return JObject.Parse( await response.Content.ReadAsStringAsync() );
    }

    private static async Task SendResponse( HttpClient client, string message )
    {
      var source = new CancellationTokenSource( new TimeSpan( 0, 0, 10 ) );

      var data = new StringContent( message, Encoding.UTF8, "text/responding" );
      var response = await client.PostAsync( host + "Response", data, source.Token );
      return;
    }



  }
}
